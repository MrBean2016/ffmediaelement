﻿namespace Unosquare.FFME.Commands
{
    using Core;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// Implements the logic to close a media stream.
    /// </summary>
    /// <seealso cref="Unosquare.FFME.Commands.MediaCommand" />
    internal sealed class CloseCommand : MediaCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloseCommand"/> class.
        /// </summary>
        /// <param name="manager">The media element.</param>
        public CloseCommand(MediaCommandManager manager)
            : base(manager, MediaCommandType.Close)
        {
            // placeholder
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        internal override void Execute()
        {
            var m = Manager.MediaElement;

            if (m.IsDisposed || m.IsOpen == false || m.IsOpening) return;

            m.Logger.Log(MediaLogMessageType.Debug, $"{nameof(CloseCommand)}: Entered");
            m.Clock.Pause();

            m.IsTaskCancellationPending = true;

            // Wait for cycles to complete.
            var waitHandles = new[] { m.BlockRenderingCycle, m.FrameDecodingCycle, m.PacketReadingCycle };
            var tasks = new[] { m.PacketReadingTask, m.FrameDecodingTask, m.BlockRenderingTask };

            m.SeekingDone.Set();
            foreach (var handle in waitHandles)
                handle.WaitOne();

            // Wait for threads to finish
            Task.WaitAll(tasks);

            // Set the threads to null
            m.BlockRenderingTask = null;
            m.FrameDecodingTask = null;
            m.PacketReadingTask = null;

            // Call close on all renderers and clear them
            foreach (var renderer in m.Renderers.Values)
                renderer.Close();

            // Remove the renderers disposing of them
            m.Renderers.Clear();

            // Reset the clock
            m.Clock.Reset();

            // Dispose the container
            if (m.Container != null)
            {
                m.Container.Dispose();
                m.Container = null;
            }

            // Dispose the Blocks for all components
            foreach (var kvp in m.Blocks) kvp.Value.Dispose();
            m.Blocks.Clear();

            // Clear the render times
            m.LastRenderTime.Clear();
            m.MediaState = System.Windows.Controls.MediaState.Close;

            // Update notification properties
            Utils.UIInvoke(DispatcherPriority.DataBind, () =>
            {
                m.NotifyPropertyChanges();
                m.ResetDependencyProperies();
            });
#if DEBUG
            if (RC.Current.InstancesByLocation.Count > 0)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Unmanaged references were left alive. This is an indication that there is a memory leak.");
                foreach (var kvp in RC.Current.InstancesByLocation)
                    builder.AppendLine($"    {kvp.Key,30}: {kvp.Value}");
                
                m.Logger.Log(MediaLogMessageType.Error, builder.ToString());
            }
#endif
            m.Logger.Log(MediaLogMessageType.Debug, $"{nameof(CloseCommand)}: Completed");
        }
    }
}
