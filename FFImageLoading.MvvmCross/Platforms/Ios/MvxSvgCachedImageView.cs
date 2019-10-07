﻿using System;
using System.Collections.Generic;
using FFImageLoading.Work;
using FFImageLoading.Cache;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using FFImageLoading.Views;
using FFImageLoading.Svg.Platform;
using Foundation;
using UIKit;
using CoreGraphics;

namespace FFImageLoading.Cross
{
            [Preserve(AllMembers = true)]
            [Register("MvxSvgCachedImageView")]
    /// <summary>
    /// MvxSvgCachedImageView by Daniel Luberda
    /// </summary>
    public class MvxSvgCachedImageView : MvxCachedImageView
    {
        public MvxSvgCachedImageView() : base() { } 
        public MvxSvgCachedImageView(IntPtr handle) : base(handle) { }
        public MvxSvgCachedImageView(CGRect frame) : base(frame) { }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ReplaceStringMap))
            {
                if (_lastImageSource != null)
                {
                    UpdateImageLoadingTask();
                }
            }
        }

        protected override void SetupOnBeforeImageLoading(TaskParameter imageLoader)
        {
            base.SetupOnBeforeImageLoading(imageLoader);

            int width = (int)this.Bounds.Width;
            int height = (int)this.Bounds.Height;

            if (width > height)
                height = 0;
            else
                width = 0;

            if ((!string.IsNullOrWhiteSpace(ImagePath) && ImagePath.IsSvgFileUrl()) || ImageStream != null)
            {
                imageLoader.WithCustomDataResolver(new SvgDataResolver(width, height, true, ReplaceStringMap));
            }
            if (!string.IsNullOrWhiteSpace(LoadingPlaceholderImagePath) && LoadingPlaceholderImagePath.IsSvgFileUrl())
            {
                imageLoader.WithCustomLoadingPlaceholderDataResolver(new SvgDataResolver(width, height, true, ReplaceStringMap));
            }
            if (!string.IsNullOrWhiteSpace(ErrorPlaceholderImagePath) && ErrorPlaceholderImagePath.IsSvgFileUrl())
            {
                imageLoader.WithCustomErrorPlaceholderDataResolver(new SvgDataResolver(width, height, true, ReplaceStringMap));
            }
        }

        Dictionary<string, string> _replaceStringMap;
        /// <summary>
        /// Used to define replacement map which will be used to
        /// replace text inside SVG file (eg. changing colors values)
        /// </summary>
        /// <value>The replace string map.</value>
        public Dictionary<string, string> ReplaceStringMap
        {
            get { return _replaceStringMap; }
            set { if (_replaceStringMap != value) { _replaceStringMap = value; OnPropertyChanged(nameof(ReplaceStringMap)); } }
        }
    }
}
