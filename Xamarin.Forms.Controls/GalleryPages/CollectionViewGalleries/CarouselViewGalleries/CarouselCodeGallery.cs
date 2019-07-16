﻿using System.Collections;

namespace Xamarin.Forms.Controls.GalleryPages.CollectionViewGalleries.CarouselViewGalleries
{
	internal class CarouselCodeGallery : ContentPage
	{
		public CarouselCodeGallery(ItemsLayoutOrientation orientation)
		{
			Title = $"CarouselView (Code, {orientation})";

			var nItems = 5;
			var layout = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Star }
				}
			};
			var itemsLayout =
			new ListItemsLayout(orientation)
			{
				SnapPointsType = SnapPointsType.MandatorySingle,
				SnapPointsAlignment = SnapPointsAlignment.Center,
				ItemSpacing = 10
			};

			var itemTemplate = ExampleTemplates.CarouselTemplate();

			var carouselView = new CarouselView
			{
				ItemsLayout = itemsLayout,
				ItemTemplate = itemTemplate,
				Position = 2,
				NumberOfVisibleItems = 1,
				Margin = new Thickness(0,10,0,40),
				PeekAreaInsets = new Thickness(30,0,30,0),
				BackgroundColor = Color.LightGray
			};

			layout.Children.Add(carouselView);

			StackLayout stacklayoutInfo = GetReadOnlyInfo(carouselView);

			var generator = new ItemsSourceGenerator(carouselView, initialItems: nItems, itemsSourceType: ItemsSourceType.ObservableCollection);

			layout.Children.Add(generator);

			var positionControl = new PositionControl(carouselView, nItems);
			layout.Children.Add(positionControl);

			layout.Children.Add(stacklayoutInfo);

			Grid.SetRow(positionControl, 1);
			Grid.SetRow(stacklayoutInfo, 2);
			Grid.SetRow(carouselView, 3);

			Content = layout;
			generator.CollectionChanged += (sender, e) => {
				positionControl.UpdatePositionCount(generator.Count);
			};

			generator.GenerateItems();
		}

		static StackLayout GetReadOnlyInfo(CarouselView carouselView)
		{
			var stacklayoutInfo = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				BindingContext = carouselView
			};

			var labelScrolling = new Label { Text = nameof(carouselView.IsScrolling) };
			var switchScrolling = new Switch { IsEnabled = false };
			switchScrolling.SetBinding(Switch.IsToggledProperty, nameof(carouselView.IsScrolling));

			var labelDragging = new Label { Text = nameof(carouselView.IsDragging) };
			var switchDragging = new Switch();
			switchDragging.SetBinding(Switch.IsToggledProperty, nameof(carouselView.IsDragging));

			var labelScrolled = new Label { Text = nameof(carouselView.Scrolled) };

			carouselView.Scrolled += (object sender, ScrolledDirectionEventArgs e) =>
			{
				labelScrolled.Text = $"{nameof(carouselView.Scrolled)}:{e.Direction} value:{e.NewValue.ToString("F")} delta: {e.Delta.ToString("F")}";
			};

			stacklayoutInfo.Children.Add(labelScrolling);
			stacklayoutInfo.Children.Add(switchScrolling);
			stacklayoutInfo.Children.Add(labelDragging);
			stacklayoutInfo.Children.Add(switchDragging);
			return new StackLayout { Children = { stacklayoutInfo, labelScrolled } };
		}
	}
}