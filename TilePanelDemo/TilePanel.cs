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
        // Attached property for child items.

        public static readonly AttachedProperty<string?> TileSizeProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, string?>("TileSize", typeof(TilePanel));

        public static string? GetTileSize(IAvaloniaObject obj)
        {
            return obj.GetValue(TileSizeProperty);
        }

        public static void SetTileSize(IAvaloniaObject obj, string? value)
        {
            obj.SetValue(TileSizeProperty, value);
        }

        // Predefined tile sizes.
        
        public static readonly StyledProperty<Size> SmallTileSizeProperty = 
            AvaloniaProperty.Register<TilePanel, Size>(nameof(SmallTileSize), new Size(228, 126));

        public static readonly StyledProperty<Size> MediumTileSizeProperty = 
            AvaloniaProperty.Register<TilePanel, Size>(nameof(MediumTileSize), new Size(228, 252));

        public static readonly StyledProperty<Size> LargeTileSizeProperty = 
            AvaloniaProperty.Register<TilePanel, Size>(nameof(LargeTileSize), new Size(456, 252));
        
        public static readonly StyledProperty<Size> WideTileSizeProperty = 
            AvaloniaProperty.Register<TilePanel, Size>(nameof(WideTileSize),new Size(684, 252));

        public Size SmallTileSize
        {
            get => GetValue(SmallTileSizeProperty);
            set => SetValue(SmallTileSizeProperty, value);
        }

        public Size MediumTileSize
        {
            get => GetValue(MediumTileSizeProperty);
            set => SetValue(MediumTileSizeProperty, value);
        }

        public Size LargeTileSize
        {
            get => GetValue(LargeTileSizeProperty);
            set => SetValue(LargeTileSizeProperty, value);
        }

        public Size WideTileSize
        {
            get => GetValue(WideTileSizeProperty);
            set => SetValue(WideTileSizeProperty, value);
        }

        // Predefined breakpoint widths.

        public static readonly StyledProperty<double> XSmallBreakpointProperty = 
            AvaloniaProperty.Register<TilePanel, double>(nameof(XSmallBreakpoint), 576);

        public static readonly StyledProperty<double> SmallBreakpointProperty = 
            AvaloniaProperty.Register<TilePanel, double>(nameof(SmallBreakpoint), 576);

        public static readonly StyledProperty<double> MediumBreakpointProperty = 
            AvaloniaProperty.Register<TilePanel, double>(nameof(MediumBreakpoint), 768);

        public static readonly StyledProperty<double> LargeBreakpointProperty = 
            AvaloniaProperty.Register<TilePanel, double>(nameof(LargeBreakpoint), 992);

        public static readonly StyledProperty<double> ExtraLargeBreakpointProperty = 
            AvaloniaProperty.Register<TilePanel, double>(nameof(ExtraLargeBreakpoint), 1200);

        public static readonly StyledProperty<double> ExtraExtraLargeBreakpointProperty = 
            AvaloniaProperty.Register<TilePanel, double>(nameof(ExtraExtraLargeBreakpoint), 1400);

        public double XSmallBreakpoint
        {
            get => GetValue(XSmallBreakpointProperty);
            set => SetValue(XSmallBreakpointProperty, value);
        }

        public double SmallBreakpoint
        {
            get => GetValue(SmallBreakpointProperty);
            set => SetValue(SmallBreakpointProperty, value);
        }

        public double MediumBreakpoint
        {
            get => GetValue(MediumBreakpointProperty);
            set => SetValue(MediumBreakpointProperty, value);
        }

        public double LargeBreakpoint
        {
            get => GetValue(LargeBreakpointProperty);
            set => SetValue(LargeBreakpointProperty, value);
        }

        public double ExtraLargeBreakpoint
        {
            get => GetValue(ExtraLargeBreakpointProperty);
            set => SetValue(ExtraLargeBreakpointProperty, value);
        }

        public double ExtraExtraLargeBreakpoint
        {
            get => GetValue(ExtraExtraLargeBreakpointProperty);
            set => SetValue(ExtraExtraLargeBreakpointProperty, value);
        }

        // Column breaks.

        public static readonly StyledProperty<int> ColumnsProperty = 
            AvaloniaProperty.Register<TilePanel, int>(nameof(Columns));

        public int Columns
        {
            get => GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        // Layout preset.
        
        public static readonly StyledProperty<string?> LayoutSizeProperty = 
            AvaloniaProperty.Register<TilePanel, string?>(nameof(LayoutSize));

        public string? LayoutSize
        {
            get => GetValue(LayoutSizeProperty);
            set => SetValue(LayoutSizeProperty, value);
        }

        // Width breakpoint source.
        
        public static readonly StyledProperty<double> WidthSourceProperty = 
            AvaloniaProperty.Register<TilePanel, Double>(nameof(WidthSource));

        public double WidthSource
        {
            get => GetValue(WidthSourceProperty);
            set => SetValue(WidthSourceProperty, value);
        }

        // ctor
        
        public TilePanel()
        {
            UpdateLayoutSizePseudoClasses(LayoutSize);
            UpdateWidthSourcePseudoClasses(WidthSource);
        } 

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ColumnsProperty)
            {
                InvalidateMeasure();
                InvalidateArrange();
            }

            if (change.Property == XSmallBreakpointProperty
                || change.Property == SmallBreakpointProperty
                || change.Property == MediumBreakpointProperty
                || change.Property == LargeBreakpointProperty
                || change.Property == ExtraLargeBreakpointProperty
                || change.Property == ExtraExtraLargeBreakpointProperty)
            {
                UpdateWidthSourcePseudoClasses(change.NewValue.GetValueOrDefault<double>());
                InvalidateMeasure();
                InvalidateArrange();
            }

            if (change.Property == LayoutSizeProperty)
            {
                UpdateLayoutSizePseudoClasses(change.NewValue.GetValueOrDefault<string>());
                InvalidateMeasure();
                InvalidateArrange();
            }

            if (change.Property == WidthSourceProperty)
            {
                UpdateWidthSourcePseudoClasses(change.NewValue.GetValueOrDefault<double>());
                InvalidateMeasure();
                InvalidateArrange();
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
            PseudoClasses.Set(":xs", widthSource < XSmallBreakpoint);
            PseudoClasses.Set(":sm", widthSource >= SmallBreakpoint);
            PseudoClasses.Set(":md", widthSource >= MediumBreakpoint);
            PseudoClasses.Set(":lg", widthSource >= LargeBreakpoint);
            PseudoClasses.Set(":xl", widthSource >= ExtraLargeBreakpoint);
            PseudoClasses.Set(":xxl", widthSource >= ExtraExtraLargeBreakpoint);
        }

        private Size MeasureArrange(bool isArrange)
        {
            double totalWidth = 0;
            double totalHeight = 0;
            int columns = 0;
            int rows = 0;
            int maxColumns = Columns;
            double horizontalOffset = 0;
            double verticalOffset = 0;
            double columnWidth = 0;
            double rowHeight = 0;

            var smallTileSize = SmallTileSize;
            var mediumTileSize = MediumTileSize;
            var largeTileSize = LargeTileSize;
            var wideTileSize = WideTileSize;
            
            foreach (var child in Children)
            {
                var tileSize = TilePanel.GetTileSize(child);

                var size = tileSize?.ToLower() switch
                {
                    "small" => smallTileSize,
                    "medium" => mediumTileSize,
                    "large" => largeTileSize,
                    "wide" => wideTileSize,
                    _ => Size.Empty
                };

                // rowHeight = Math.Max(rowHeight, size.Height);
                rowHeight = size.Height;
                columnWidth = size.Width;

                columns++;
                rows++;

                totalWidth = Math.Max(totalWidth, columnWidth);
                totalHeight += rowHeight;

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

                // horizontalOffset += size.Width;
                verticalOffset += rowHeight;
            }

            Debug.WriteLine($"CxR: {columns}x{rows}");
            
            return new Size(totalWidth, totalHeight);
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