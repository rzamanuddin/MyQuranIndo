using MyQuranIndo.Controls;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.ViewModels.Juz;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Juz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageJuzDetailPage : BindableTabbedPage
    {
        TabbedPageJuzDetailViewModel _viewModel;

        public TabbedPageJuzDetailPage()
        {
            InitializeComponent();

            // 1. Buat ViewModel-nya terlebih dahulu
            _viewModel = new TabbedPageJuzDetailViewModel();

            // 2. JANGAN pasang BindingContext dulu! Panggil BuildForm untuk mengisi data tab.
            BuildForm();
        }

        private async void BuildForm()
        {
            // 3. Tunggu sampai data koleksi Tabs selesai dibuat di memory background
            await _viewModel.Initialization;

            // 4. Setelah data SIAP, baru pasang BindingContext ke UI.
            // Cara ini membuat Xamarin langsung menggambar halaman dengan posisi tab yang sudah matang.
            BindingContext = _viewModel;

            // 5. Berikan sedikit jeda lalu paksa highlight visualnya
            await Task.Delay(250);
            ForceHighlightTab();
        }

        private void ForceHighlightTab()
        {
            int jId = 0;
            if (int.TryParse(_viewModel.JuzID, out jId) && jId > 0)
            {
                // Hitung indeks berdasarkan urutan terbalik (114 ke 1)
                int targetIndex = Math.Abs(jId - JuzHeader.JUZ_COUNT);

                if (this.Children.Count > targetIndex)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // Jeda 300ms untuk memastikan UI thread Android benar-benar 'idle' (santai)
                        // await Task.Delay(300);

                        // Lakukan pemindahan visual sebanyak 2 kali (Trick untuk mereset TabLayout Android)
                        // Pindahkan ke indeks 0 sebentar, lalu langsung tembak ke indeks target
                        if (targetIndex != 0)
                        {
                            this.CurrentPage = this.Children[0];
                            // await Task.Delay(50);
                        }

                        // Tembak ke halaman asli yang dituju
                        var targetPage = this.Children[targetIndex];
                        this.CurrentPage = targetPage;

                        // Sinkronisasikan ulang ke ViewModel
                        _viewModel.SelectedTab = targetPage;
                    });
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage?.BindingContext is JuzDetailViewModel vm)
            {
                _viewModel.JuzID = vm.JuzID;
                _viewModel.SurahID = vm.SurahID;
                _ = _viewModel.LoadJuzDetail();
            }
        }
    }
}