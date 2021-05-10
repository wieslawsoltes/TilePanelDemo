using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace TilePanelDemo
{
    [PseudoClasses(
        ":small", ":medium", ":large", ":wide",
        ":xs", ":sm", ":md", ":lg", ":xl", ":xxl")]
    public class TilePanel : Panel
    {
        // Tile sizes
        private Size _smallSize = new Size(228, 126);
        private Size _mediumSize = new Size(228, 252);
        private Size _largeSize = new Size(456, 252);
        private Size _wideSize = new Size(684, 252);

        // Width breakpoints
        private double _xSmallBreakpoint = 576;
        private double _smallBreakpoint = 576;
        private double _mediumBreakpoint = 768;
        private double _largeBreakpoint = 992;
        private double _extraLargeBreakpoint = 1200;
        private double _extraExtraLargeBreakpoint = 1400;

        public static readonly AttachedProperty<string?> TileSizeProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, string?>("TileSize", typeof(TilePanel));

        public static readonly StyledProperty<int> MaxColumnsProperty = 
            AvaloniaProperty.Register<TilePanel, int>(nameof(MaxColumns));

        public static readonly StyledProperty<string?> LayoutSizeProperty = 
            AvaloniaProperty.Register<TilePanel, string?>(nameof(LayoutSize));

        public static readonly StyledProperty<double> WidthSourceProperty = 
            AvaloniaProperty.Register<TilePanel, Double>(nameof(WidthSource));

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
        
        public double WidthSource
        {
            get => GetValue(WidthSourceProperty);
            set => SetValue(WidthSourceProperty, value);
        }

        public TilePanel()
        {
            UpdateLayoutSizePseudoClasses(LayoutSize);
            UpdateWidthSourcePseudoClasses(WidthSource);
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

            if (change.Property == WidthSourceProperty)
            {
                UpdateWidthSourcePseudoClasses(change.NewValue.GetValueOrDefault<double>());
            }
        }

        private void UpdateLayoutSizePseudoClasses(string? layoutSize)
        {
            PseudoClasses.Set(":small", string.Compare(layoutSize, "Small", StringComparison.OrdinalIgnoreCase) == 0);
            PseudoClasses.Set(":medium", string.Compare(layoutSize, "Medium", StringComparison.OrdinalIgnoreCase) == 0);
            PseudoClasses.Set(":large", string.Compare(layoutSize, "Large", StringComparison.OrdinalIgnoreCase) == 0);
            PseudoClasses.Set(":wide", string.Compare(layoutSize, "Wide", StringComparison.OrdinalIgnoreCase) == 0);
        }

        private void UpdateWidthSourcePseudoClasses(double widthSource)
        {
            PseudoClasses.Set(":xs", widthSource < _xSmallBreakpoint);
            PseudoClasses.Set(":sm", widthSource >= _smallBreakpoint);
            PseudoClasses.Set(":md", widthSource >= _mediumBreakpoint);
            PseudoClasses.Set(":lg", widthSource >= _largeBreakpoint);
            PseudoClasses.Set(":xl", widthSource >= _extraLargeBreakpoint);
            PseudoClasses.Set(":xxl", widthSource >= _extraExtraLargeBreakpoint);
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

                var size = tileSize?.ToLower() switch
                {
                    "small" => _smallSize,
                    "medium" => _mediumSize,
                    "large" => _largeSize,
                    "wide" => _wideSize,
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