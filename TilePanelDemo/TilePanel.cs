﻿using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace TilePanelDemo
{
    [PseudoClasses(":small", ":medium", ":large")]
    public class TilePanel : Panel
    {
        private Size _smallSize = new Size(228, 126);
        private Size _mediumSize = new Size(228, 252);
        private Size _largeSize = new Size(456, 252);

        public static readonly AttachedProperty<string?> TileSizeProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, string?>("TileSize", typeof(TilePanel));

        public static readonly StyledProperty<int> MaxColumnsProperty = 
            AvaloniaProperty.Register<TilePanel, int>(nameof(MaxColumns));

        public static readonly StyledProperty<string?> LayoutSizeProperty = 
            AvaloniaProperty.Register<TilePanel, string?>(nameof(LayoutSize));

        public static string? GetTileSize(IAvaloniaObject obj)
        {
            return obj.GetValue(TileSizeProperty);
        }

        public static void SetTileSize(IAvaloniaObject obj, string? value)
        {
            obj.SetValue(TileSizeProperty, value);
        }

        public int MaxColumns
        {
            get => GetValue(MaxColumnsProperty);
            set => SetValue(MaxColumnsProperty, value);
        }

        public string? LayoutSize
        {
            get => GetValue(LayoutSizeProperty);
            set => SetValue(LayoutSizeProperty, value);
        }

        public TilePanel()
        {
            UpdateLayoutSizePseudoClasses(LayoutSize);
        } 
        
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MaxColumnsProperty)
            {
                InvalidateMeasure();
                InvalidateArrange();
            }
            
            if (change.Property == LayoutSizeProperty)
            {
                UpdateLayoutSizePseudoClasses(change.NewValue.GetValueOrDefault<string>());
            }
        }

        private void UpdateLayoutSizePseudoClasses(string? layoutSize)
        {
            PseudoClasses.Set(":small", string.Compare(layoutSize, "Small", StringComparison.OrdinalIgnoreCase) == 0);
            PseudoClasses.Set(":medium", string.Compare(layoutSize, "Medium", StringComparison.OrdinalIgnoreCase) == 0);
            PseudoClasses.Set(":large", string.Compare(layoutSize, "Large", StringComparison.OrdinalIgnoreCase) == 0);
        }

        private Size MeasureArrange(bool isArrange)
        {
            double totalHeight = 0;
            int columns = 0;
            int rows = 0;
            int maxColumns = MaxColumns;
            double horizontalOffset = 0;
            double verticalOffset = 0;
            double rowHeight = 0;

            foreach (var child in Children)
            {
                var tileSize = TilePanel.GetTileSize(child);

                var size = (tileSize?.ToLower() ?? "small") switch
                {
                    "small" => _smallSize,
                    "medium" => _mediumSize,
                    "large" => _largeSize,
                    _ => Size.Empty
                };

                columns++;

                if (columns > maxColumns)
                {
                    columns = 0;
                    horizontalOffset = 0;
                    verticalOffset += rowHeight;
                    rowHeight = 0;
                    rows++;
                }

                if (isArrange)
                {
                    var x = horizontalOffset;
                    var y = verticalOffset;
                    var rect = new Rect(new Point(x, y), size);
                    child.Arrange(rect);
                }
                else
                {
                    child.Measure(size);
                }

                rowHeight = Math.Max(rowHeight, size.Height);
                horizontalOffset += size.Width;
            }

            totalHeight = verticalOffset + rowHeight;
   
            Debug.WriteLine($"CxR: {columns}x{rows}");
            
            return new Size(horizontalOffset, totalHeight);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return MeasureArrange(isArrange: false);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return MeasureArrange(isArrange: true);
        }
    }
}