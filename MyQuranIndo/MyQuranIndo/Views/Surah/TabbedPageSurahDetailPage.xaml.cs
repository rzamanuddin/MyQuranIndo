using MyQuranIndo.Controls;
using MyQuranIndo.ViewModels.Surah;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Surah
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageSurahDetailPage : BindableTabbedPage
    {
        TabbedPageSurahDetailViewModel _viewModel;
        private bool _isFirstLoad = true;

        public TabbedPageSurahDetailPage()
        {
            InitializeComponent();

            // 1. Buat ViewModel-nya terlebih dahulu
            _viewModel = new TabbedPageSurahDetailViewModel();

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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

            //// Jika di BuildForm terlalu cepat, kita coba lagi saat halaman muncul
            //if (_isFirstLoad && _viewModel.Tabs.Count > 0)
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Task.Delay(200); // Beri waktu rendering Android selesai
            //        ForceHighlightTab();
            //        _isFirstLoad = false;
            //    });
            //}
        }

        private void ForceHighlightTab()
        {
            int sID = 0;
            if (int.TryParse(_viewModel.SurahID, out sID) && sID > 0)
            {
                // Hitung indeks berdasarkan urutan terbalik (114 ke 1)
                int targetIndex = Math.Abs(sID - Models.Qurans.Surah.SURAH_COUNT);

                if (this.Children.Count > targetIndex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // Jeda 300ms untuk memastikan UI thread Android benar-benar 'idle' (santai)
                        //await Task.Delay(300);

                        // Lakukan pemindahan visual sebanyak 2 kali (Trick untuk mereset TabLayout Android)
                        // Pindahkan ke indeks 0 sebentar, lalu langsung tembak ke indeks target
                        if (targetIndex != 0)
                        {
                            this.CurrentPage = this.Children[0];
                            //await Task.Delay(50);
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

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage?.BindingContext is SurahDetailViewModel detailVM)
            {
                _viewModel.SurahID = detailVM.SurahID;
                _ = _viewModel.LoadSurahDetail();
            }
        }
    }
}