using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyQuranIndo.Controls
{
    public class BindableTabbedPage : TabbedPage
    {
        public BindableTabbedPage()
        {
            
        }

        public static BindableProperty ChildrenListProperty = BindableProperty.Create<BindableTabbedPage, IList>(o => o.ChildrenList, new List<Page>(), propertyChanged: UpdateList);

        public IList ChildrenList
        {
            get { return (IList)GetValue(ChildrenListProperty); }
            set { SetValue(ChildrenListProperty, value); }
        }

        private static void UpdateList(BindableObject bindable, IList oldValue, IList newValue)
        {
            var tabbedPage = bindable as BindableTabbedPage;
            if (tabbedPage != null)
            {
                tabbedPage.Children.Clear();
                foreach (var page in newValue)
                {
                    tabbedPage.Children.Add((Page)page);
                }
            }
        }

        //private static void UpdateList(BindableObject bindable, IList oldValue, IList newValue)
        //{
        //    var tabbedPage = bindable as BindableTabbedPage;
        //    if (tabbedPage != null && newValue != null)
        //    {
        //        // 1. Matikan update layout sementara (jika memungkinkan)
        //        tabbedPage.Children.Clear();

        //        // 2. Masukkan semua page ke dalam array/list lokal dulu
        //        foreach (var page in newValue)
        //        {
        //            tabbedPage.Children.Add((Page)page);
        //        }

        //        // 3. Pemicu paksa sinkronisasi visual setelah Children benar-benar terisi
        //        if (tabbedPage.BindingContext is TabbedPageSurahDetailViewModel vm)
        //        {
        //            Device.BeginInvokeOnMainThread(async () =>
        //            {
        //                // Beri waktu napas bagi internal renderer Android untuk mengenali semua tab
        //                await System.Threading.Tasks.Task.Delay(300);

        //                int sID = 0;
        //                if (int.TryParse(vm.SurahID, out sID) && sID > 0)
        //                {
        //                    int targetIndex = Math.Abs(sID - Surah.SURAH_COUNT); // Sesuaikan dengan jumlah total surat Anda
        //                    if (tabbedPage.Children.Count > targetIndex)
        //                    {
        //                        // Set CurrentPage dari internal Children Xamarin, bukan dari ObservableCollection
        //                        tabbedPage.CurrentPage = tabbedPage.Children[targetIndex];
        //                    }
        //                }
        //            });
        //        }
        //    }
        //}

    }
}
