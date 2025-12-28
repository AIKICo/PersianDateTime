using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using DevExpress.Data.Mask;
using DevExpress.Data.Mask.Internal;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Popups;
using FarsiLibrary.Localization;
using FarsiLibrary.Utils;

namespace FarsiLibrary.Win.DevExpress
{
    /// <summary>
    /// کلاس کمکی برای دسترسی به اعضای داخلی DevExpress که دسترسی مستقیم به آنها نداریم.
    /// </summary>
    public static class DevExpressReflectionHelper
    {
        public static T GetPropertyValue<T>(object obj, string propName)
        {
            if (obj == null) return default(T);
            var prop = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop == null) return default(T);
            return (T)prop.GetValue(obj, null);
        }

        public static T GetFieldValue<T>(object obj, string fieldName)
        {
            if (obj == null) return default(T);
            // جستجو در کلاس جاری و کلاس‌های پدر
            var type = obj.GetType();
            FieldInfo field = null;
            while (type != null)
            {
                field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField);
                if (field != null) break;
                type = type.BaseType;
            }

            if (field == null) return default(T);
            return (T)field.GetValue(obj);
        }

        public static object InvokeMethod(object obj, string methodName, params object[] args)
        {
            if (obj == null) return null;
            var type = obj.GetType();
            MethodInfo method = null;
            while (type != null)
            {
                method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (method != null) break;
                type = type.BaseType;
            }

            if (method == null) return null;
            return method.Invoke(obj, args);
        }
    }

    public class FAMinsItemsProvider : MinsItemsProvider
    {
        public FAMinsItemsProvider(int count) : base(count) { }

        protected override IItemPainter CreatePainter(int itemIndex)
        {
            return new FAMinsItemsPainter();
        }
    }

    public class FASecondsItemsProvider : SecondsItemsProvider
    {
        protected override IItemPainter CreatePainter(int itemIndex)
        {
            return new FASecondsItemsPainter();
        }

        public FASecondsItemsProvider(int count) : base(count) { }
    }

    public class FAHoursItemsProvider : HoursItemsProvider
    {
        public FAHoursItemsProvider(int count) : base(count) { }

        protected override IItemPainter CreatePainter(int itemIndex)
        {
            return new FAHoursItemsPainter(StartIndex);
        }
    }

    public class FAMeridiemItemsProvider : MeridiemItemsProvider, IItemsProvider
    {
        public FAMeridiemItemsProvider(int count) : base(count) { }

        IItemPainter IItemsProvider.GetItemPainter(int itemIndex)
        {
            return new FAMeridiemItemsPainter();
        }
    }

    public class FAYearItemsProvider : YearItemsProvider, IItemsProvider
    {
        public FAYearItemsProvider(int count) : base(count) { }

        IItemPainter IItemsProvider.GetItemPainter(int itemIndex)
        {
            return new FAYearItemsPainter();
        }
    }

    public class FADaysItemsProvider : DaysItemsProvider
    {
        public FADaysItemsProvider(int count) : base(count) { }

        protected override IItemPainter CreatePainter()
        {
            return new FADaysItemsPainter();
        }
    }

    public class FAMonthItemsProvider : MonthItemsProvider, IItemsProvider
    {
        public FAMonthItemsProvider(int count) : base(count) { }

        IItemPainter IItemsProvider.GetItemPainter(int itemIndex)
        {
            return new FAMonthItemsPainter();
        }
    }

    public class FAYearItemsPainter : YearItemsPainter
    {
        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            PickItemsPainter painter = new PickItemsPainter();
            string firstString = GetYear(info);
            painter.DrawItem(cache, drawInfo, info, firstString, string.Empty);
        }

        protected string GetYear(PickItemInfo info)
        {
            // اصلاح شده: استفاده از Reflection برای گرفتن SelectedDate
            var calendarObj = GetCalendar(info);
            DateTime date = DevExpressReflectionHelper.GetPropertyValue<DateTime>(calendarObj, "SelectedDate");

            // جلوگیری از خطای تاریخ مینیمم
            if (date == DateTime.MinValue) date = DateTime.Now;

            int year = info.ItemIndex + 1;
            int month = date.Month;
            int day = date.Day;

            // اصلاح منطق برای جلوگیری از روزهای نامعتبر (مثلا 31ام ماه که در تقویم شمسی نباشد)
            try
            {
                PersianDate pd = new DateTime(year, month, day);
                return toFarsi.Convert(pd.Year.ToString());
            }
            catch
            {
                // در صورت خطا، فقط سال را تبدیل کن (حالت fallback)
                return toFarsi.Convert(year.ToString());
            }
        }
    }

    public class FAMonthItemsPainter : MonthItemsPainter
    {
        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            PickItemsPainter painter = new PickItemsPainter();
            var pd = GetDate(info);
            var monthNo = toFarsi.Convert(pd.Month.ToString());
            var monthName = PersianDateTimeFormatInfo.AbbreviatedMonthGenitiveNames[pd.Month - 1];

            // اصلاح شده: استفاده از Reflection برای ShowTime
            var calendarObj = GetCalendar(info);
            bool showTime = (bool)DevExpressReflectionHelper.InvokeMethod(calendarObj, "ShowTime");

            bool descriptionIsExist = showTime;
            string firstString = showTime ? monthNo : monthName;
            string description = descriptionIsExist && painter.ShouldDrawDescription(info) ? monthName : string.Empty;
            painter.DrawItem(cache, drawInfo, info, firstString, description);
        }

        protected PersianDate GetDate(PickItemInfo info)
        {
            // اصلاح شده
            var calendarObj = GetCalendar(info);
            DateTime date = DevExpressReflectionHelper.GetPropertyValue<DateTime>(calendarObj, "SelectedDate");
            if (date == DateTime.MinValue) date = DateTime.Now;

            int year = date.Year;
            int month = info.ItemIndex + 1;
            int day = date.Day;

            // مدیریت خطای روز نامعتبر در ماه
            try
            {
                date = new DateTime(year, month, day);
            }
            catch
            {
                date = new DateTime(year, month, 1);
            }

            return date;
        }
    }

    public class FADaysItemsPainter : DaysItemsPainter
    {
        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            var painter = new PickItemsPainter();
            var date = GetDate(info);
            var firstString = toFarsi.Convert(date.Day.ToString());

            // اصلاح شده
            var calendarObj = GetCalendar(info);
            bool showTime = (bool)DevExpressReflectionHelper.InvokeMethod(calendarObj, "ShowTime");

            bool descriptionIsExist = showTime;
            string description = descriptionIsExist && painter.ShouldDrawDescription(info) ? date.LocalizedWeekDayName : string.Empty;
            painter.DrawItem(cache, drawInfo, info, firstString, description);
        }

        protected PersianDate GetDate(PickItemInfo info)
        {
            // اصلاح شده
            var calendarObj = GetCalendar(info);
            DateTime date = DevExpressReflectionHelper.GetPropertyValue<DateTime>(calendarObj, "SelectedDate");
            if (date == DateTime.MinValue) date = DateTime.Now;

            int year = date.Year;
            int month = date.Month;
            int day = info.ItemIndex + 1;

            try
            {
                date = new DateTime(year, month, day);
            }
            catch
            {
                // اگر روز در ماه میلادی معتبر نبود
                date = new DateTime(year, month, 1);
            }

            return date;
        }
    }

    public class FAMeridiemItemsPainter : MeridiemItemsPainter
    {
        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            PickItemsPainter painter = new PickItemsPainter();
            // ShowTime اینجا استفاده نشده ولی اگر لازم بود با رفلکشن بگیرید
            // bool descriptionIsExist = GetCalendar(info).ShowTime(); 
            string firstString = info.ItemIndex == 0 ? PersianDateTimeFormatInfo.AMDesignator : PersianDateTimeFormatInfo.PMDesignator;
            painter.DrawItem(cache, drawInfo, info, firstString, string.Empty);
        }
    }

    public class FAHoursItemsPainter : HoursItemsPainter
    {
        private readonly int startIndex;
        private readonly BaseLocalizer localizer;

        public FAHoursItemsPainter(int startIndex) : base(startIndex)
        {
            this.startIndex = startIndex;
            this.localizer = Localization.FALocalizeManager.Instance.GetLocalizer();
        }

        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            PickItemsPainter painter = new PickItemsPainter();
            string firstString = toFarsi.Convert(painter.ConvertIntToString(info.ItemIndex + startIndex, StringLength));
            string description = painter.ShouldDrawDescription(info) ? localizer.GetLocalizedString(StringID.Hour) : string.Empty;
            painter.DrawItem(cache, drawInfo, info, firstString, description);
        }
    }

    public class FASecondsItemsPainter : SecondsItemsPainter
    {
        private BaseLocalizer localizer;

        public FASecondsItemsPainter()
        {
            this.localizer = Localization.FALocalizeManager.Instance.GetLocalizer();
        }

        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            PickItemsPainter painter = new PickItemsPainter();
            int secondIncrement = 1;
            string firstString = toFarsi.Convert(painter.ConvertIntToString(info.ItemIndex * secondIncrement, StringLength));
            string description = painter.ShouldDrawDescription(info) ? (localizer.GetLocalizedString(StringID.Second)) : string.Empty;
            painter.DrawItem(cache, drawInfo, info, firstString, description);
        }

    }

    public class FAMinsItemsPainter : MinsItemsPainter
    {
        private readonly BaseLocalizer localizer;

        public FAMinsItemsPainter()
        {
            this.localizer = Localization.FALocalizeManager.Instance.GetLocalizer();
        }

        protected override void DrawCore(GraphicsCache cache, PickItemInfo info, IPickItemsContainerDrawInfo drawInfo)
        {
            PickItemsPainter painter = new PickItemsPainter();
            int minuteIncrement = 1;
            string firstString = toFarsi.Convert(painter.ConvertIntToString(info.ItemIndex * minuteIncrement, StringLength));
            string description = painter.ShouldDrawDescription(info) ? localizer.GetLocalizedString(StringID.Minute) : string.Empty;
            painter.DrawItem(cache, drawInfo, info, firstString, description);
        }
    }

    [ToolboxItem(false)]
    public class FADateEditTouchCalendar : DateEditTouchCalendar
    {
        public FADateEditTouchCalendar(FATouchPopupDateEditForm form) : base(form)
        {
        }

        int firstTimeProviderIndex = -1;

        // متد AddNewProvider در نسخه شما وجود ندارد یا virtual نیست.
        // override را حذف کردیم. برای اینکه این متد کار کند، باید جایی فراخوانی شود.
        // معمولاً کلاس پایه متد CreateDefaultProviders دارد، شاید لازم باشد آن را دستکاری کنید.
        public void AddNewProvider(DateTimeMaskFormatElementEditable editableFormat)
        {
            // استفاده از Reflection برای متدهای داخلی
            bool showTime = (bool)DevExpressReflectionHelper.InvokeMethod(this, "ShowTime");

            if (Form == null || Form.DateEdit == null || !showTime)
            {
                bool isTimeProvider = (bool)DevExpressReflectionHelper.InvokeMethod(this, "IsTimeProvider", editableFormat);
                if (isTimeProvider) return;
            }

            IItemsProvider provider = CreateNewFarsiProvider(editableFormat);
            if (provider != null)
            {
                // دسترسی به لیست Providers با Reflection
                IList providersList = DevExpressReflectionHelper.GetFieldValue<IList>(this, "providers"); // نام فیلد ممکن است "providers" یا "Providers" باشد
                if (providersList == null) providersList = DevExpressReflectionHelper.GetPropertyValue<IList>(this, "Providers");

                if (providersList != null)
                {
                    // دسترسی به متد ShouldInsertProvider
                    bool shouldInsert = (bool)DevExpressReflectionHelper.InvokeMethod(this, "ShouldInsertProvider", provider);

                    if (shouldInsert)
                        providersList.Insert(firstTimeProviderIndex, provider);
                    else
                        providersList.Add(provider);

                    bool isTimeProvider = (bool)DevExpressReflectionHelper.InvokeMethod(this, "IsTimeProvider", editableFormat);
                    if (isTimeProvider)
                    {
                        if (firstTimeProviderIndex == -1) firstTimeProviderIndex = providersList.Count - 1;
                        // تنظیم فیلد IsTimeProviderAdded
                        // نام فیلد ممکن است با حروف کوچک شروع شود یا backing field باشد
                        SetPrivateField("isTimeProviderAdded", true);
                    }

                    // افزایش TotalProviders
                    int total = DevExpressReflectionHelper.GetPropertyValue<int>(this, "TotalProviders");
                    SetPrivateProperty("TotalProviders", total + 1);
                }
            }
        }

        private void SetPrivateField(string fieldName, object value)
        {
            var field = typeof(DateEditTouchCalendar).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null) field.SetValue(this, value);
        }

        private void SetPrivateProperty(string propName, object value)
        {
            var prop = typeof(DateEditTouchCalendar).GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null) prop.SetValue(this, value, null);
        }

        protected IItemsProvider CreateNewFarsiProvider(DateTimeMaskFormatElementEditable editableFormat)
        {
            // گرفتن مقادیر Increment از طریق Reflection
            int minIncrement = (int)(DevExpressReflectionHelper.InvokeMethod(this, "GetMinuteIncrement") ?? 1);
            int secIncrement = (int)(DevExpressReflectionHelper.InvokeMethod(this, "GetSecondIncrement") ?? 1);

            if (editableFormat is DateTimeMaskFormatElement_h12)
            {
                FAHoursItemsProvider hoursItemsProvider = new FAHoursItemsProvider(12);
                hoursItemsProvider.StartIndex = 1;
                return hoursItemsProvider;
            }
            if (editableFormat is DateTimeMaskFormatElement_H24)
                return new FAHoursItemsProvider(24);
            if (editableFormat is DateTimeMaskFormatElement_d)
                return new FADaysItemsProvider(31);
            if (editableFormat is DateTimeMaskFormatElement_Min)
                return new FAMinsItemsProvider(60 / minIncrement);
            if (editableFormat is DateTimeMaskFormatElement_Month)
                return new FAMonthItemsProvider(12);
            if (editableFormat is DateTimeMaskFormatElement_s)
                return new FASecondsItemsProvider(60 / secIncrement);
            if (editableFormat is DateTimeMaskFormatElement_Year)
                return new FAYearItemsProvider(9999);
            if (editableFormat is DateTimeMaskFormatElement_AmPm)
                return new FAMeridiemItemsProvider(2);
            return null;
        }
    }

    public class FATouchPopupDateEditForm : TouchPopupDateEditForm
    {
        public FATouchPopupDateEditForm(PopupBaseEdit ownerEdit) : base(ownerEdit)
        {
        }

        protected override void CreateTouchCalendar()
        {
            TouchCalendar = new FADateEditTouchCalendar(this);
        }
    }
}