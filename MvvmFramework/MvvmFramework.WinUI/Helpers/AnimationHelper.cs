using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;

namespace MvvmFramework.WinUI;

public static class AnimationHelper
{
	public static bool ExpandHeight(FrameworkElement animatedControl, int minSize = 0)
	{
		double requestedHeight;
		bool isMaxHeight;

		if(animatedControl.DesiredSize.Height > animatedControl.MaxHeight)
		{
			// *Is min size

			// Set to max size
			requestedHeight = animatedControl.DesiredSize.Height;
			isMaxHeight = false;
		}
		else
		{
			// *Is max size

			// Set to min size
			requestedHeight = minSize;
			isMaxHeight = true;
		}

		var storyboard = new Storyboard();
		var animation = new DoubleAnimation
		{
			To = requestedHeight,
			Duration = new(TimeSpan.FromMilliseconds(150)),
			EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
			EnableDependentAnimation = true,
		};

		Storyboard.SetTarget(storyboard, animatedControl);
		Storyboard.SetTargetProperty(animation, nameof(FrameworkElement.MaxHeight));
		storyboard.Children.Add(animation);

		storyboard.Begin();

		return isMaxHeight;
	}

	public static void AnimateMaxHeight(this FrameworkElement element, double targetHeight)
	{
		var storyboard = new Storyboard();
		var animation = new DoubleAnimation
		{
			//From = target.ActualHeight,
			To = targetHeight,
			Duration = new(TimeSpan.FromMilliseconds(150)),
			EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
			EnableDependentAnimation = true,
		};

		Storyboard.SetTarget(storyboard, element);
		Storyboard.SetTargetProperty(animation, nameof(FrameworkElement.MaxHeight));
		storyboard.Children.Add(animation);

		storyboard.Begin();
	}

	public static void AnimateOpacity(this FrameworkElement element, double opacity, double duration = 500)
	{
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            To = opacity,
            Duration = new(TimeSpan.FromMilliseconds(duration)),
            EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
            EnableDependentAnimation = true,
        };

        Storyboard.SetTarget(storyboard, element);
        Storyboard.SetTargetProperty(animation, nameof(FrameworkElement.Opacity));
        storyboard.Children.Add(animation);

        storyboard.Begin();
    }

	public static void AnimateMaterialise(this FrameworkElement element)
	{
        var storyboard = new Storyboard();

        var scaleXFrames = new DoubleAnimationUsingKeyFrames()
        {
            KeyFrames =
            {
                new DiscreteDoubleKeyFrame()
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMicroseconds(0)),
                    Value = 1,
                },
                new SplineDoubleKeyFrame()
                {
                    KeyTime = (KeyTime)TimeSpan.Parse((string)Application.Current.Resources["ControlFastAnimationDuration"]),
                    KeySpline = new KeySpline() { ControlPoint1 = new(0, 0), ControlPoint2 = new(0, 1) },
                    Value = 1.05,
                },
            }
        };

        var scaleYFrames = new DoubleAnimationUsingKeyFrames()
        {
            KeyFrames =
            {
                new DiscreteDoubleKeyFrame()
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMicroseconds(0)),
                    Value = 1,
                },
                new SplineDoubleKeyFrame()
                {
                    KeyTime = (KeyTime)TimeSpan.Parse((string)Application.Current.Resources["ControlFastAnimationDuration"]),
                    KeySpline = new KeySpline() { ControlPoint1 = new(0, 0), ControlPoint2 = new(0, 1) },
                    Value = 1.05,
                },
            }
        };

        var opacityFrames = new DoubleAnimationUsingKeyFrames()
        {
            KeyFrames =
            {
                new DiscreteDoubleKeyFrame()
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMicroseconds(0)),
                    Value = 0,
                },
                new LinearDoubleKeyFrame()
                {
                    KeyTime = (KeyTime)TimeSpan.Parse((string)Application.Current.Resources["ControlFasterAnimationDuration"]),
                    Value = 1,
                },
            }
        };

        Storyboard.SetTargetProperty(scaleXFrames, "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)");
        Storyboard.SetTargetProperty(scaleYFrames, "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)");
        Storyboard.SetTargetProperty(opacityFrames, "Opacity");

        storyboard.Children.Add(scaleXFrames);
        storyboard.Children.Add(scaleYFrames);
        storyboard.Children.Add(opacityFrames);

        Storyboard.SetTarget(storyboard, element);

        storyboard.Begin();
    }
}
