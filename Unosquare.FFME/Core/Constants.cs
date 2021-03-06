﻿namespace Unosquare.FFME.Core
{
    using FFmpeg.AutoGen;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Defines library-wide constants
    /// </summary>
    internal static class Constants
    {
        public const string CodecOptionThreads = "threads";
        public const string CodecOptionRefCountedFrames = "refcounted_frames";
        public const string CodecOptionLowRes = "lowres";

        public const double DefaultSpeedRatio = 1.0d;
        public const double DefaultBalance = 0.0d;
        public const double DefaultVolume = 1.0d;

        public const double MinSpeedRatio = 0.0d;
        public const double MaxSpeedRatio = 8.0d;

        public const double MinBalance = -1.0d;
        public const double MaxBalance = 1.0d;

        public const double MaxVolume = 1.0d;
        public const double MinVolume = 0.0d;

        public const string DllAVCodec = "avcodec-57.dll";
        public const string DllAVFilter = "avfilter-6.dll";
        public const string DllAVFormat = "avformat-57.dll";
        public const string DllAVUtil = "avutil-55.dll";
        public const string DllSWResample = "swresample-2.dll";
        public const string DllSWScale = "swscale-4.dll";
        public const string DllAVDevice = "avdevice-57.dll";

        public const FrameworkPropertyMetadataOptions AffectsMeasureAndRender 
            = FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender;

        public static readonly ReadOnlyCollection<MediaType> MediaTypes
            = new ReadOnlyCollection<MediaType>(Enum.GetValues(typeof(MediaType)).Cast<MediaType>().ToArray());

        public static readonly string[] LiveStreamUrlPrefixes = new[] { "rtp:", "udp:" };
        public static readonly string[] LiveStreamFormatNames = new[] { "rtp", "rtsp", "sdp" };
        public static readonly TimeSpan UIPropertyUpdateInterval = TimeSpan.FromMilliseconds(50);

        public static readonly ReadOnlyDictionary<int, MediaLogMessageType> FFmpegLogLevels = new ReadOnlyDictionary<int, MediaLogMessageType>(
            new Dictionary<int, MediaLogMessageType>
            {
                { ffmpeg.AV_LOG_DEBUG, MediaLogMessageType.Debug },
                { ffmpeg.AV_LOG_ERROR, MediaLogMessageType.Error },
                { ffmpeg.AV_LOG_FATAL, MediaLogMessageType.Error },
                { ffmpeg.AV_LOG_INFO, MediaLogMessageType.Info },
                { ffmpeg.AV_LOG_PANIC, MediaLogMessageType.Error },
                { ffmpeg.AV_LOG_TRACE, MediaLogMessageType.Trace },
                { ffmpeg.AV_LOG_WARNING, MediaLogMessageType.Warning },
            });
    }
}
