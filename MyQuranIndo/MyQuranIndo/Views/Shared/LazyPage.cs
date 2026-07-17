using System;
using Xamarin.Forms;

public class LazyPage : ContentPage
{
    private readonly Func<Page> _factory;
    private bool _created;

    public LazyPage(Func<Page> factory, string title = null)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        Title = title;
        // placeholder minimal agar TabLayout punya item
        Content = new StackLayout { HeightRequest = 1 };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_created) return;
        _created = true;

        var real = _factory();
        if (real == null) return;

        Device.BeginInvokeOnMainThread(() =>
        {
            if (Parent is TabbedPage tabParent)
            {
                var idx = tabParent.Children.IndexOf(this);
                if (idx >= 0)
                {
                    // keep the same title/icon if needed
                    real.Title = real.Title ?? this.Title;
                    tabParent.Children[idx] = real;
                    // optional: set current page to the created one
                    tabParent.CurrentPage = real;
                }
            }
        });
    }

    public System.Threading.Tasks.Task EnsureCreatedAsync()
    {
        var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        Device.BeginInvokeOnMainThread(() =>
        {
            try
            {
                if (_created)
                {
                    tcs.SetResult(true);
                    return;
                }

                _created = true;
                var real = _factory();
                if (real == null)
                {
                    tcs.SetResult(true);
                    return;
                }

                if (Parent is TabbedPage tabParent)
                {
                    var idx = tabParent.Children.IndexOf(this);
                    if (idx >= 0)
                    {
                        real.Title = real.Title ?? this.Title;
                        tabParent.Children[idx] = real;
                        tabParent.CurrentPage = real;
                    }
                }

                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }
}