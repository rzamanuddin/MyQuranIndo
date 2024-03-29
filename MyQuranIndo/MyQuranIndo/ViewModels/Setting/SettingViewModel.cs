﻿using Android.Widget;
using MyQuranIndo.Configuration;
using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Fonts;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.Services;
using MyQuranIndo.Themes;
using MyQuranIndo.Views;
using MyQuranIndo.Views.About;
using MyQuranIndo.Views.Setting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Setting
{
    public class SettingViewModel : BaseViewModel
    {
        private bool isActiveTransliteration;
        private bool isActiveTranslate;

        public bool IsActiveTransliteration
        {
            get
            {
                isActiveTransliteration = Preferences.Get(References.Setting.IS_ACTIVE_TRANSLITERATION, true);
                return isActiveTransliteration;
            }
            set
            {
                Preferences.Set(References.Setting.IS_ACTIVE_TRANSLITERATION, value);
                SetProperty(ref isActiveTransliteration, value);
            }
        }

        public bool IsActiveTranslate
        {
            get
            {
                isActiveTranslate = Preferences.Get(References.Setting.IS_ACTIVE_TRANSLATE, true);
                return isActiveTranslate;
            }
            set
            {
                Preferences.Set(References.Setting.IS_ACTIVE_TRANSLATE, value);
                SetProperty(ref isActiveTranslate, value);
            }
        }

        private string bismillahSample;
        public string BismillahSample
        {
            get
            {
                return FontHelper.GetBismillah();
            }

            set => SetProperty(ref bismillahSample, value);
        }

        private string bismillahTranslateSample;
        public string BismillahTranslateSample
        {
            get
            {
                return FontHelper.GetBismillahTranslate();
            }

            set => SetProperty(ref bismillahTranslateSample, value);
        }

        private string bismillahTextIndoSample;
        public string BismillahTextIndoSample
        {
            get
            {
                return FontHelper.GetBismillahTextIndo();
            }

            set => SetProperty(ref bismillahTextIndoSample, value);
        }

        //private ResourceDictionary theme;
        //public ResourceDictionary Theme
        //{
        //    get
        //    {
        //        if (App.Current.Properties.ContainsKey(References.Setting.THEME))
        //        {
        //            return (ResourceDictionary)App.Current.Properties[References.Setting.THEME];
        //        }
        //        else
        //        {
        //            return new BlueTheme();
        //        }
        //    }
        //    set
        //    {
        //        App.Current.Properties[References.Setting.THEME] = value;
        //        SetProperty(ref theme, value);
        //    }
        //}

        // public ObservableCollection<Theme> Themes { get; private set; }

        public Command AboutTapped { get; }
        public ICommand ClearCacheTapped { get; }
        public ICommand OpenAppStoreTapped { get; }
        public ICommand ColorThemeCommand
        {
            get;
            //{
            //    return new Command<string>((theme) => OnColorThemeSelected(theme));
            //}
        }
        public ICommand FontSizeCommand
        {
            get;
            //{
            //    return new Command<string>((theme) => OnColorThemeSelected(theme));
            //}
        }

        public ICommand FontSizeTranslateCommand
        {
            get;
        }

        public ICommand FontSizeTextIndoCommand
        {
            get;
        }

        public ICommand ReciterCommand
        {
            get;
        }

        public ICommand TafsirCommand
        {
            get;
        }

        public ICommand RasmCommand
        {
            get;
        }

        public ObservableCollection<KeyValuePair<int, string>> Tafsirs { get; private set; }

        private KeyValuePair<int, string> tafsirSelected;
        public KeyValuePair<int, string> TafsirSelected
        {
            get
            {
                int tafsir = Preferences.Get(References.Setting.TAFSIR_SELECTED, (int)Models.Qurans.TafsirType.Kemenag);
                tafsirSelected = Tafsirs.FirstOrDefault(q => q.Key == tafsir);
                return tafsirSelected;
            }
            set
            {
                if (tafsirSelected.Key != value.Key)
                {
                    Preferences.Set(References.Setting.TAFSIR_SELECTED, value.Key);
                    SetProperty(ref tafsirSelected, value);
                }

                //ReciterURL = ReciterHelper.GetReciterUrl();
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> Rasms { get; private set; }

        private KeyValuePair<int, string> rasmSelected;
        public KeyValuePair<int, string> RasmSelected
        {
            get
            {
                int rasm = Preferences.Get(References.Setting.RASM_SELECTED, (int)Models.Qurans.RasmType.IndoPak);
                rasmSelected = Tafsirs.FirstOrDefault(q => q.Key == rasm);
                return rasmSelected;
            }
            set
            {
                if (rasmSelected.Key != value.Key)
                {
                    Preferences.Set(References.Setting.RASM_SELECTED, value.Key);
                    SetProperty(ref rasmSelected, value);
                }

                //ReciterURL = ReciterHelper.GetReciterUrl();
            }
        }


        public ObservableCollection<KeyValuePair<int, String>> Reciters { get; private set; }

        private KeyValuePair<int, string> reciterSelected;
        public KeyValuePair<int, string> ReciterSelected
        {
            get
            {
                int reciter = Preferences.Get(References.Setting.RECITER_SELECTED, (int)Models.Qurans.Reciter.AsSudais);
                reciterSelected = Reciters.FirstOrDefault(q => q.Key == reciter);
                return reciterSelected;
            }
            set
            {
                if (reciterSelected.Key != value.Key)
                {
                    Preferences.Set(References.Setting.RECITER_SELECTED, value.Key);
                    SetProperty(ref reciterSelected, value);
                }

                //ReciterURL = ReciterHelper.GetReciterUrl();
            }
        }

        //private string reciterURL;
        //public string ReciterURL
        //{
        //    get => reciterURL;
        //    set => SetProperty(ref reciterURL, value);
        //}

        private Color tafsirKemenagColor;
        public Color TafsirKemenagColor
        {
            get
            {
                if (Preferences.Get(References.Setting.TAFSIR_SELECTED, 0) == (int)TafsirType.Kemenag)
                {
                    tafsirKemenagColor = Color.LightGray;
                }
                else
                {
                    tafsirKemenagColor = Color.White;
                }

                return tafsirKemenagColor;
            }

            set
            {
                SetProperty(ref tafsirKemenagColor, value);
            }
        }

        private Color tafsirAlJalalainColor;
        public Color TafsirAlJalalainColor
        {
            get
            {
                if (Preferences.Get(References.Setting.TAFSIR_SELECTED, 0) == (int)TafsirType.AlJalalain)
                {
                    tafsirAlJalalainColor = Color.LightGray;
                }
                else
                {
                    tafsirAlJalalainColor = Color.White;
                }

                return tafsirAlJalalainColor;
            }

            set
            {
                SetProperty(ref tafsirAlJalalainColor, value);
            }
        }

        private Color rasmUtsmaniColor;
        public Color RasmUtsmaniColor
        {
            get
            {
                if (Preferences.Get(References.Setting.RASM_SELECTED, 0) == (int)RasmType.Utsmani)
                {
                    rasmUtsmaniColor = Color.LightGray;
                }
                else
                {
                    rasmUtsmaniColor = Color.White;
                }

                return rasmUtsmaniColor;
            }

            set
            {
                SetProperty(ref rasmUtsmaniColor, value);
            }
        }

        private Color rasmIndoPakColor;
        public Color RasmIndoPakColor
        {
            get
            {
                if (Preferences.Get(References.Setting.RASM_SELECTED, 0) == (int)RasmType.IndoPak)
                {
                    rasmIndoPakColor = Color.LightGray;
                }
                else
                {
                    rasmIndoPakColor = Color.White;
                }

                return rasmIndoPakColor;
            }

            set
            {
                SetProperty(ref rasmIndoPakColor, value);
            }
        }


        private Color reciterAsSudaisColor;
        public Color ReciterAsSudaisColor
        {
            get
            {
                if (Preferences.Get(References.Setting.RECITER_SELECTED, 0) == (int)Reciter.AsSudais)
                {
                    reciterAsSudaisColor = Color.LightGray;
                }
                else
                {
                    reciterAsSudaisColor = Color.White;
                }

                return reciterAsSudaisColor;
            }

            set
            {
                SetProperty(ref reciterAsSudaisColor, value);
            }
        }

        private Color reciterAlAfasyColor;
        public Color ReciterAlAfasyColor
        {
            get
            {
                if (Preferences.Get(References.Setting.RECITER_SELECTED, 0) == (int)Reciter.AlAfasy)
                {
                    reciterAlAfasyColor = Color.LightGray;
                }
                else
                {
                    reciterAlAfasyColor = Color.White;
                }

                return reciterAlAfasyColor;
            }

            set
            {
                SetProperty(ref reciterAlAfasyColor, value);
            }
        }


        private Color reciterAlMatroudColor;
        public Color ReciterAlMatroudColor
        {
            get
            {
                if (Preferences.Get(References.Setting.RECITER_SELECTED, 0) == (int)Reciter.AlMatroud)
                {
                    reciterAlMatroudColor = Color.LightGray;
                }
                else
                {
                    reciterAlMatroudColor = Color.White;
                }

                return reciterAlMatroudColor;
            }

            set
            {
                SetProperty(ref reciterAlMatroudColor, value);
            }
        }



        public ObservableCollection<KeyValuePair<int, String>> FontSizes { get; private set; }
        public ObservableCollection<KeyValuePair<int, String>> FontSizeTranslates { get; private set; }
        public ObservableCollection<KeyValuePair<int, String>> FontSizeTextIndos { get; private set; }


        private KeyValuePair<int, string> fontSizeSelected;
        public KeyValuePair<int, string> FontSizeSelected
        {
            get
            {
                int fontStyle = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Small);  
                fontSizeSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontStyle);
                return fontSizeSelected;
            }
            set
            {
                if (fontSizeSelected.Key != value.Key)
                {
                    Preferences.Set(References.Setting.FONT_SIZE_SELECTED, value.Key);
                    SetProperty(ref fontSizeSelected, value);
                }
                FontSizeArabic = FontHelper.GetFontSizeArabic(true);
            }
        }

        private KeyValuePair<int, string> fontSizeTranslateSelected;
        public KeyValuePair<int, string> FontSizeTranslateSelected
        {
            get
            {
                int fontStyle = Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Caption);
                fontSizeTranslateSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontStyle);
                return fontSizeTranslateSelected;
            }
            set
            {
                if (fontSizeTranslateSelected.Key != value.Key)
                {
                    Preferences.Set(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, value.Key);
                    SetProperty(ref fontSizeTranslateSelected, value);
                }
                FontSizeTranslate = FontHelper.GetFontSizeTranslate();
            }
        }

        private KeyValuePair<int, string> fontSizeTextIndoSelected;
        public KeyValuePair<int, string> FontSizeTextIndoSelected
        {
            get
            {
                int fontStyle = Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Caption);
                fontSizeTextIndoSelected = FontSizeTextIndos.FirstOrDefault(q => q.Key == fontStyle);
                return fontSizeTextIndoSelected;
            }
            set
            {
                if (fontSizeTextIndoSelected.Key != value.Key)
                {
                    Preferences.Set(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, value.Key);
                    SetProperty(ref fontSizeTextIndoSelected, value);
                }
                FontSizeTextIndo = FontHelper.GetFontSizeTextIndo();
            }
        }

        private double fontSizeTranslate;
        public double FontSizeTranslate
        {
            get => fontSizeTranslate;
            set => SetProperty(ref fontSizeTranslate, value);
        }


        private double fontSizeTextIndo;
        public double FontSizeTextIndo
        {
            get => fontSizeTextIndo;
            set => SetProperty(ref fontSizeTextIndo, value);
        }

        private double fontSizeArabic;
        public double FontSizeArabic
        {
            get => fontSizeArabic;
            set => SetProperty(ref fontSizeArabic, value);
        }

        private string fontArabicName;
        public string FontArabicName
        {
            get
            {               
                return FontHelper.GetFontArabicName();
            }

            set => SetProperty(ref fontArabicName, value);
        }



        private Color fontSizeSmallColor;
        public Color FontSizeSmallColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_SELECTED, 0) == (int)FontSize.Small)
                {
                    fontSizeSmallColor =  Color.LightGray;
                }
                else
                {
                    fontSizeSmallColor = Color.White;
                }

                return fontSizeSmallColor;
            }

            set
            {
                SetProperty(ref fontSizeSmallColor, value);
            }
        }

        private Color fontSizeMediumColor;
        public Color FontSizeMediumColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_SELECTED, 0) == (int)FontSize.Medium)
                {
                    fontSizeMediumColor = Color.LightGray;
                }
                else
                {
                    fontSizeMediumColor = Color.White;
                }

                return fontSizeMediumColor;
            }

            set
            {
                SetProperty(ref fontSizeMediumColor, value);
            }
        }

        private Color fontSizeLargeColor;
        public Color FontSizeLargeColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_SELECTED, 0) == (int)FontSize.Large)
                {
                    fontSizeLargeColor = Color.LightGray;
                }
                else
                {
                    fontSizeLargeColor = Color.White;
                }
                return fontSizeLargeColor;
            }

            set
            {
                SetProperty(ref fontSizeLargeColor, value);
            }
        }

        private Color fontSizeTranslatecaptionColor;
        public Color FontSizeTranslateCaptionColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, 3) == (int)FontSize.Caption)
                {
                    fontSizeTranslatecaptionColor = Color.LightGray;
                }
                else
                {
                    fontSizeTranslatecaptionColor = Color.White;
                }

                return fontSizeTranslatecaptionColor;
            }

            set
            {
                SetProperty(ref fontSizeTranslatecaptionColor, value);
            }
        }

        private Color fontSizeTranslateSmallColor;
        public Color FontSizeTranslateSmallColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, 3) == (int)FontSize.Small)
                {
                    fontSizeTranslateSmallColor = Color.LightGray;
                }
                else
                {
                    fontSizeTranslateSmallColor = Color.White;
                }

                return fontSizeTranslateSmallColor;
            }

            set
            {
                SetProperty(ref fontSizeTranslateSmallColor, value);
            }
        }

        private Color fontSizeTranslateMediumColor;
        public Color FontSizeTranslateMediumColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, 3) == (int)FontSize.Medium)
                {
                    fontSizeTranslateMediumColor = Color.LightGray;
                }
                else
                {
                    fontSizeTranslateMediumColor = Color.White;
                }

                return fontSizeTranslateMediumColor;
            }

            set
            {
                SetProperty(ref fontSizeTranslateMediumColor, value);
            }
        }

        private Color fontSizeTranslateLargeColor;
        public Color FontSizeTranslateLargeColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, 3) == (int)FontSize.Large)
                {
                    fontSizeTranslateLargeColor = Color.LightGray;
                }
                else
                {
                    fontSizeTranslateLargeColor = Color.White;
                }
                return fontSizeTranslateLargeColor;
            }

            set
            {
                SetProperty(ref fontSizeTranslateLargeColor, value);
            }
        }

        private Color fontSizeTextIndocaptionColor;
        public Color FontSizeTextIndoCaptionColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, 3) == (int)FontSize.Caption)
                {
                    fontSizeTextIndocaptionColor = Color.LightGray;
                }
                else
                {
                    fontSizeTextIndocaptionColor = Color.White;
                }

                return fontSizeTextIndocaptionColor;
            }

            set
            {
                SetProperty(ref fontSizeTextIndocaptionColor, value);
            }
        }

        private Color fontSizeTextIndoSmallColor;
        public Color FontSizeTextIndoSmallColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, 3) == (int)FontSize.Small)
                {
                    fontSizeTextIndoSmallColor = Color.LightGray;
                }
                else
                {
                    fontSizeTextIndoSmallColor = Color.White;
                }

                return fontSizeTextIndoSmallColor;
            }

            set
            {
                SetProperty(ref fontSizeTextIndoSmallColor, value);
            }
        }

        private Color fontSizeTextIndoMediumColor;
        public Color FontSizeTextIndoMediumColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, 3) == (int)FontSize.Medium)
                {
                    fontSizeTextIndoMediumColor = Color.LightGray;
                }
                else
                {
                    fontSizeTextIndoMediumColor = Color.White;
                }

                return fontSizeTextIndoMediumColor;
            }

            set
            {
                SetProperty(ref fontSizeTextIndoMediumColor, value);
            }
        }

        private Color fontSizeTextIndoLargeColor;
        public Color FontSizeTextIndoLargeColor
        {
            get
            {
                if (Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, 3) == (int)FontSize.Large)
                {
                    fontSizeTextIndoLargeColor = Color.LightGray;
                }
                else
                {
                    fontSizeTextIndoLargeColor = Color.White;
                }
                return fontSizeTextIndoLargeColor;
            }

            set
            {
                SetProperty(ref fontSizeTextIndoLargeColor, value);
            }
        }

        //public Task Initialization { get; private set; }

        public SettingViewModel()
        {
            AboutTapped = new Command(OnAboutSelected);
            OpenAppStoreTapped = new Command(async () => await OnOpenAppStore());
            ColorThemeCommand = new Command<int>((theme) => OnColorThemeSelected(theme));
            FontSizeCommand = new Command<int>((fontSize) => OnFontSizeSelected(fontSize));
            FontSizeTranslateCommand = new Command<int>((fontSize) => OnFontSizeTranslateSelected(fontSize));
            FontSizeTextIndoCommand = new Command<int>((fontSize) => OnFontSizeTextIndoSelected(fontSize));
            ReciterCommand = new Command<int>((reciter) => OnReciterSelected(reciter));
            TafsirCommand = new Command<int>((tafsir) => OnTafsirSelected(tafsir));
            RasmCommand = new Command<int>((rasm) => OnRasmSelected(rasm));
            ClearCacheTapped = new Command(async () => await OnClearCacheSelected());

            FontSizes = new ObservableCollection<KeyValuePair<int, string>>();
            FontSizeTranslates = new ObservableCollection<KeyValuePair<int, string>>();
            FontSizeTextIndos = new ObservableCollection<KeyValuePair<int, string>>();

            Reciters = new ObservableCollection<KeyValuePair<int, string>>();
            Tafsirs = new ObservableCollection<KeyValuePair<int, string>>();
            Rasms = new ObservableCollection<KeyValuePair<int, string>>();

            //int fontStyle = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Small);
            //FontSizeSelected = FontSizes.FirstOrDefault(q => q.Key == fontStyle);
            //Themes.Add(new Theme() { ID = 1, Name = "Biru" });
            //Themes.Add(new Theme() { ID = 1, Name = "Hijau" });

            //Device.GetNamedSize(NamedSize.Large, typeof(Label));
        }

        private void OnReciterSelected(int reciter)
        {
            int reciterSelected = Preferences.Get(References.Setting.RECITER_SELECTED, 0);
            switch (reciter)
            {
                case (int)Models.Qurans.Reciter.AsSudais:
                    if (reciterSelected != (int)Reciter.AsSudais)
                    {
                        Preferences.Set(References.Setting.RECITER_SELECTED, (int)Reciter.AsSudais);
                        ReciterSelected = Reciters.FirstOrDefault(q => q.Key == reciter);
                        ReciterAsSudaisColor = Color.LightGray;
                        ReciterAlAfasyColor = Color.White;
                        ReciterAlMatroudColor = Color.White;
                    }
                    ToastService.Show($"Qari Syekh As-Sudais berhasil dipilih", false);
                    break;
                case (int)Models.Qurans.Reciter.AlAfasy:
                    if (reciterSelected != (int)Reciter.AlAfasy)
                    {
                        Preferences.Set(References.Setting.RECITER_SELECTED, (int)Reciter.AlAfasy);
                        ReciterSelected = Reciters.FirstOrDefault(q => q.Key == reciter);
                        ReciterAsSudaisColor = Color.White;
                        ReciterAlAfasyColor = Color.LightGray;
                        ReciterAlMatroudColor = Color.White;
                    }
                    ToastService.Show($"Qari Syekh Al-Afasy berhasil dipilih", false);
                    break;
                case (int)Models.Qurans.Reciter.AlMatroud:
                    if (reciterSelected != (int)Reciter.AlMatroud)
                    {
                        Preferences.Set(References.Setting.RECITER_SELECTED, (int)Reciter.AlMatroud);
                        ReciterSelected = Reciters.FirstOrDefault(q => q.Key == reciter);
                        ReciterAsSudaisColor = Color.White;
                        ReciterAlAfasyColor = Color.White;
                        ReciterAlMatroudColor = Color.LightGray;
                    }
                    ToastService.Show($"Qari Syekh Al-Mathrud berhasil dipilih", false);
                    break;
                default:
                    goto case (int)Models.Qurans.Reciter.AsSudais;
            }
        }

        private void OnTafsirSelected(int tafsir)
        {
            int tasfirSelected = Preferences.Get(References.Setting.TAFSIR_SELECTED, 0);
            switch (tafsir)
            {
                case (int)Models.Qurans.TafsirType.Kemenag:
                    if (tasfirSelected != (int)TafsirType.Kemenag)
                    {
                        Preferences.Set(References.Setting.TAFSIR_SELECTED, (int)TafsirType.Kemenag);
                        TafsirSelected = Tafsirs.FirstOrDefault(q => q.Key == tafsir);
                        TafsirKemenagColor = Color.LightGray;
                        TafsirAlJalalainColor = Color.White;
                    }
                    ToastService.Show($"Tafsir Kemenag berhasil dipilih", false);
                    break;
                case (int)Models.Qurans.TafsirType.AlJalalain:
                    if (tasfirSelected != (int)TafsirType.AlJalalain)
                    {
                        Preferences.Set(References.Setting.TAFSIR_SELECTED, (int)TafsirType.AlJalalain);
                        TafsirSelected = Tafsirs.FirstOrDefault(q => q.Key == tafsir);
                        TafsirKemenagColor = Color.White;
                        TafsirAlJalalainColor = Color.LightGray;
                    }
                    ToastService.Show($"Tafsir AlJalalain berhasil dipilih", false);
                    break;
                default:
                    goto case (int)Models.Qurans.TafsirType.Kemenag;
            }
        }

        private async Task OnClearCacheSelected()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string[] filePaths = Directory.GetFiles(folder, "*.mp3");

            if (filePaths.Length > 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    string filePath = filePaths[i];
                    File.Delete(filePath);
                }
                ToastService.Show($"Hapus {filePaths.Length} berkas berhasil", false);
            }
            else
            {
                ToastService.Show($"Berkas sampah tidak ditemukan", false);
            }
        }

        private void OnRasmSelected(int rasm)
        {
            int rasmSelected = Preferences.Get(References.Setting.RASM_SELECTED, 0);
            int fontSizeSelected = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, 0);

            switch (rasm)
            {
                case (int)Models.Qurans.RasmType.IndoPak:
                    if (rasmSelected != (int)RasmType.IndoPak)
                    {
                        Preferences.Set(References.Setting.RASM_SELECTED, (int)RasmType.IndoPak);
                        RasmSelected = Rasms.FirstOrDefault(q => q.Key == rasm);
                        RasmIndoPakColor = Color.LightGray;
                        RasmUtsmaniColor = Color.White;
                    }
                    ToastService.Show($"Rasm IndoPak berhasil dipilih", false);
                    break;
                case (int)Models.Qurans.RasmType.Utsmani:
                    if (rasmSelected != (int)RasmType.Utsmani)
                    {
                        Preferences.Set(References.Setting.RASM_SELECTED, (int)RasmType.Utsmani);
                        RasmSelected = Rasms.FirstOrDefault(q => q.Key == rasm);
                        RasmIndoPakColor = Color.White;
                        RasmUtsmaniColor = Color.LightGray;
                    }
                    ToastService.Show($"Rasm Utsmani berhasil dipilih", false);
                    break;
                default:
                    goto case (int)Models.Qurans.RasmType.Utsmani;
            }

            FontArabicName = FontHelper.GetFontArabicName();
            BismillahSample = FontHelper.GetBismillah();
            FontSizeSelected = FontSizes.FirstOrDefault(q => q.Key == fontSizeSelected);
        }

        private void OnFontSizeSelected(int fontSize)
        {
            int fontSizeSelected = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, 0);
            switch (fontSize)
            {
                case (int)FontSize.Small:
                    if (fontSizeSelected != (int)FontSize.Small)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Small);
                        FontSizeSelected = FontSizes.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeSmallColor = Color.LightGray;
                        FontSizeMediumColor = Color.White;
                        FontSizeLargeColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks kecil berhasil dipilih", false);
                    break;
                case (int)FontSize.Medium:
                    if (fontSizeSelected != (int)FontSize.Medium)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Medium);
                        FontSizeSelected = FontSizes.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeMediumColor = Color.LightGray;
                        FontSizeLargeColor = Color.White;
                        FontSizeSmallColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks sedang berhasil dipilih", false);
                    break;
                case (int)FontSize.Large:
                    if (fontSizeSelected != (int)FontSize.Large)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Large);
                        FontSizeSelected = FontSizes.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeLargeColor = Color.LightGray;
                        FontSizeMediumColor = Color.White;
                        FontSizeSmallColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks besar berhasil dipilih", false);
                    break;
                default:
                    goto case (int)FontSize.Small;
            }
        }

        private void OnFontSizeTranslateSelected(int fontSize)
        {
            int fontSizeTranslateSelected = Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, 3);
            switch (fontSize)
            {
                case (int)FontSize.Caption:
                    if (fontSizeTranslateSelected != (int)FontSize.Caption)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Caption);
                        FontSizeTranslateSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTranslateSmallColor = Color.White;
                        FontSizeTranslateMediumColor = Color.White;
                        FontSizeTranslateLargeColor = Color.White;
                        FontSizeTranslateCaptionColor = Color.LightGray;
                    }
                    ToastService.Show($"Ukuran teks default berhasil dipilih", false);
                    break;
                case (int)FontSize.Small:
                    if (fontSizeTranslateSelected != (int)FontSize.Small)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Small);
                        FontSizeTranslateSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTranslateSmallColor = Color.LightGray;
                        FontSizeTranslateMediumColor = Color.White;
                        FontSizeTranslateLargeColor = Color.White;
                        FontSizeTranslateCaptionColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks kecil berhasil dipilih", false);
                    break;
                case (int)FontSize.Medium:
                    if (fontSizeTranslateSelected != (int)FontSize.Medium)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Medium);
                        FontSizeTranslateSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTranslateMediumColor = Color.LightGray;
                        FontSizeTranslateLargeColor = Color.White;
                        FontSizeTranslateSmallColor = Color.White;
                        FontSizeTranslateCaptionColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks sedang berhasil dipilih", false);
                    break;
                case (int)FontSize.Large:
                    if (fontSizeTranslateSelected != (int)FontSize.Large)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Large);
                        FontSizeTranslateSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTranslateLargeColor = Color.LightGray;
                        FontSizeTranslateMediumColor = Color.White;
                        FontSizeTranslateSmallColor = Color.White;
                        FontSizeTranslateCaptionColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks besar berhasil dipilih", false);
                    break;
                default:
                    goto case (int)FontSize.Caption;
            }
        }

        private void OnFontSizeTextIndoSelected(int fontSize)
        {
            int fontSizeTranslateSelected = Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, 3);
            switch (fontSize)
            {
                case (int)FontSize.Caption:
                    if (fontSizeTranslateSelected != (int)FontSize.Caption)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Caption);
                        FontSizeTextIndoSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTextIndoSmallColor = Color.White;
                        FontSizeTextIndoMediumColor = Color.White;
                        FontSizeTextIndoLargeColor = Color.White;
                        FontSizeTextIndoCaptionColor = Color.LightGray;
                    }
                    ToastService.Show($"Ukuran teks default berhasil dipilih", false);
                    break;
                case (int)FontSize.Small:
                    if (fontSizeTranslateSelected != (int)FontSize.Small)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Small);
                        FontSizeTextIndoSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTextIndoSmallColor = Color.LightGray;
                        FontSizeTextIndoMediumColor = Color.White;
                        FontSizeTextIndoLargeColor = Color.White;
                        FontSizeTextIndoCaptionColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks kecil berhasil dipilih", false);
                    break;
                case (int)FontSize.Medium:
                    if (fontSizeTranslateSelected != (int)FontSize.Medium)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Medium);
                        FontSizeTextIndoSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTextIndoMediumColor = Color.LightGray;
                        FontSizeTextIndoLargeColor = Color.White;
                        FontSizeTextIndoSmallColor = Color.White;
                        FontSizeTextIndoCaptionColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks sedang berhasil dipilih", false);
                    break;
                case (int)FontSize.Large:
                    if (fontSizeTranslateSelected != (int)FontSize.Large)
                    {
                        Preferences.Set(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Large);
                        FontSizeTextIndoSelected = FontSizeTranslates.FirstOrDefault(q => q.Key == fontSize);
                        FontSizeTextIndoLargeColor = Color.LightGray;
                        FontSizeTextIndoMediumColor = Color.White;
                        FontSizeTextIndoSmallColor = Color.White;
                        FontSizeTextIndoCaptionColor = Color.White;
                    }
                    ToastService.Show($"Ukuran teks besar berhasil dipilih", false);
                    break;
                default:
                    goto case (int)FontSize.Caption;
            }
        }

        public async Task LoadDataAsync()
        {
            FontSizes.Clear();
            FontSizes.Add(new KeyValuePair<int, string>((int)FontSize.Small, "Kecil"));
            FontSizes.Add(new KeyValuePair<int, string>((int)FontSize.Medium, "Sedang"));
            FontSizes.Add(new KeyValuePair<int, string>((int)FontSize.Large, "Besar"));

            FontSizeTranslates.Clear();
            FontSizeTranslates.Add(new KeyValuePair<int, string>((int)FontSize.Small, "Default"));
            FontSizeTranslates.Add(new KeyValuePair<int, string>((int)FontSize.Small, "Kecil"));
            FontSizeTranslates.Add(new KeyValuePair<int, string>((int)FontSize.Medium, "Sedang"));
            FontSizeTranslates.Add(new KeyValuePair<int, string>((int)FontSize.Large, "Besar"));

            FontSizeTextIndos.Clear();
            FontSizeTextIndos.Add(new KeyValuePair<int, string>((int)FontSize.Small, "Default"));
            FontSizeTextIndos.Add(new KeyValuePair<int, string>((int)FontSize.Small, "Kecil"));
            FontSizeTextIndos.Add(new KeyValuePair<int, string>((int)FontSize.Medium, "Sedang"));
            FontSizeTextIndos.Add(new KeyValuePair<int, string>((int)FontSize.Large, "Besar"));

            int fontStyle = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Small);
            FontSizeSelected = FontSizes.FirstOrDefault(q => q.Key == fontStyle);
            fontStyle = Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Caption);
            FontSizeTranslateSelected = FontSizes.FirstOrDefault(q => q.Key == fontStyle);
            fontStyle = Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Caption);
            FontSizeTextIndoSelected = FontSizes.FirstOrDefault(q => q.Key == fontStyle);

            Reciters.Clear();
            Reciters.Add(new KeyValuePair<int, string>((int)Models.Qurans.Reciter.AsSudais, AppSetting.GetUrlMP3()));
            Reciters.Add(new KeyValuePair<int, string>((int)Models.Qurans.Reciter.AlAfasy, AppSetting.GetUrlMP3AlAfasy()));
            Reciters.Add(new KeyValuePair<int, string>((int)Models.Qurans.Reciter.AlMatroud, AppSetting.GetUrlMP3AlMatroud()));

            int reciter = Preferences.Get(References.Setting.RECITER_SELECTED, (int)Models.Qurans.Reciter.AsSudais);
            ReciterSelected = Reciters.FirstOrDefault(q => q.Key == reciter);

            Tafsirs.Clear();
            Tafsirs.Add(new KeyValuePair<int, string>((int)Models.Qurans.TafsirType.Kemenag, "Kemenag"));
            Tafsirs.Add(new KeyValuePair<int, string>((int)Models.Qurans.TafsirType.AlJalalain, "Al-Jalalain"));

            int tafsir = Preferences.Get(References.Setting.TAFSIR_SELECTED, (int)Models.Qurans.TafsirType.Kemenag);
            TafsirSelected = Tafsirs.FirstOrDefault(q => q.Key == tafsir);
        }

        //private async Task InitStart()
        //{
        //    //FontSizes = new ObservableCollection<KeyValuePair<int, string>>();
        //    //FontSizes.Add(new KeyValuePair<int, string>((int)FontSize.Small, "Kecil"));
        //    //FontSizes.Add(new KeyValuePair<int, string>((int)FontSize.Medium, "Sedang"));
        //    //FontSizes.Add(new KeyValuePair<int, string>((int)FontSize.Large, "Besar"));
        //}

        private void OnColorThemeSelected(int theme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null & mergedDictionaries.Count > 0)
            {
                mergedDictionaries.Clear();
                int themeSetting = Preferences.Get(References.Setting.THEME, 0);

                try
                {
                    switch (theme)
                    {
                        case (int)ThemeStyle.Blue:
                            //if (themeSetting != (int)ThemeStyle.Blue)
                            //{
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Blue);
                            mergedDictionaries.Add(new BlueTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Blue);
                            ToastService.Show($"Tema warna biru berhasil dipilih", false);
                            //}
                            break;
                        case (int)ThemeStyle.Green:
                            //if (themeSetting != (int)ThemeStyle.Green)
                            //{
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Green);
                            mergedDictionaries.Add(new GreenTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Green);
                            ToastService.Show($"Tema warna hijau berhasil dipilih", false);
                            //}
                            break;
                        case (int)ThemeStyle.Orange:
                            //if (themeSetting != (int)ThemeStyle.Orange)
                            //{
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Orange);
                            mergedDictionaries.Add(new OrangeTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Orange);
                            ToastService.Show($"Tema warna oranye berhasil dipilih", false);
                            //}
                            break;
                        case (int)ThemeStyle.Black:
                            // if (themeSetting != (int)ThemeStyle.Black)
                            // {
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Black);
                            mergedDictionaries.Add(new BlackTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Black);
                            ToastService.Show($"Tema warna hitam berhasil dipilih", false);
                            //}
                            break;
                        case (int)ThemeStyle.Purple:
                            //if (themeSetting != (int)ThemeStyle.Purple)
                            //{
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Purple);
                            mergedDictionaries.Add(new PurpleTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Purple);
                            ToastService.Show($"Tema warna ungu berhasil dipilih", false);
                            //}
                            break;
                        case (int)ThemeStyle.Pink:
                            //if (themeSetting != (int)ThemeStyle.Pink)
                            //{
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Pink);
                            mergedDictionaries.Add(new PinkTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Pink);
                            ToastService.Show($"Tema warna pink berhasil dipilih", false);
                            //}
                            break;
                        case (int)ThemeStyle.Red:
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.Red);
                            mergedDictionaries.Add(new RedTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.Red);
                            ToastService.Show($"Tema warna merah berhasil dipilih", false);
                            break;
                        case (int)ThemeStyle.DarkBlue:
                            Preferences.Set(References.Setting.THEME, (int)ThemeStyle.DarkBlue);
                            mergedDictionaries.Add(new DarkBlueTheme());

                            MessagingCenter.Send(this, Message.MSG_KEY_CHANGE_THEME, (int)ThemeStyle.DarkBlue);
                            ToastService.Show($"Tema warna biru dongker berhasil dipilih", false);
                            break;
                        default:
                            goto case (int)ThemeStyle.Blue;
                    }
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }        

        private async Task OnOpenAppStore()
        {
            string url = string.Empty;

            if (Device.RuntimePlatform == Device.Android)
            {
                url = @"https://play.google.com/store/apps/details?id=com.rommyzamanuddin.myquranindo";
            }
            await Browser.OpenAsync(url, BrowserLaunchMode.External);
        }
        private async void OnAboutSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(AboutPage)}");
        }
    }
}
