using System;
using System.ComponentModel;
using System.Globalization;
using DevExpress.Data.Mask;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using FarsiLibrary.Utils;
using FarsiLibrary.Utils.Internals;
using PersianCalendar = FarsiLibrary.Utils.PersianCalendar;

namespace FarsiLibrary.Win.DevExpress
{
    public class XtraFADateEdit : DateEdit
    {
        public const string EditorName = "XtraFADateEdit";

        static XtraFADateEdit()
        {
            Register();
        }

        public bool ShouldSerializeNullDateCalendarValue()
        {
            return false;
        }

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(XtraFADateEdit), typeof(RepositoryItemXtraFADateEdit), typeof(DateEditViewInfo), new ButtonEditPainter(), true, EditImageIndexes.DateEdit));
        }

        public override string EditorTypeName
        {
            get { return EditorName; }
        }

        protected override MaskManager CreateMaskManager(MaskProperties mask)
        {
            return new PersianDateTimeMaskManager();
        }

        protected override PopupBaseForm CreatePopupForm()
        {
            // تغییر: حذف شرط IsFarsiCulture
            // همیشه پاپ‌آپ شمسی باز شود
            if (Properties.CalendarView == CalendarView.TouchUI)
                return new FATouchPopupDateEditForm(this);

            return new VistaPopupPersianDateEditForm(this);
        }
    }

    // در فایل XtraFADateEdit.cs

    [UserRepositoryItem("Register")]
    public class RepositoryItemXtraFADateEdit : RepositoryItemDateEdit
    {
        // ۱. اطمینان از ثبت کنترل
        static RepositoryItemXtraFADateEdit()
        {
            Register();
        }

        public RepositoryItemXtraFADateEdit()
        {
            // ۲. غیرفعال کردن استفاده از ماسک برای فرمت نمایشی
            // این کار باعث می‌شود کنترلر مجبور شود از منطق GetDisplayText ما استفاده کند
            this.UseMaskAsDisplayFormat = false;

            // ۳. تنظیم دکمه پیش‌فرض
            // EnsureDefaultButton = true;

        }

        public const string EditorName = "XtraFADateEdit";

        public override string EditorTypeName => EditorName;

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(XtraFADateEdit), typeof(RepositoryItemXtraFADateEdit), typeof(DateEditViewInfo), new ButtonEditPainter(), true, EditImageIndexes.DateEdit));
        }

        // ***************************************************************
        // قلب تپنده رفع مشکل: مداخله در آخرین لحظه نمایش متن
        // ***************************************************************
        protected override void RaiseCustomDisplayText(CustomDisplayTextEventArgs e)
        {
            // فراخوانی منطق پایه
            base.RaiseCustomDisplayText(e);

            // اگر مقدار معتبر است، آن را به شمسی تبدیل کن
            // این متد بر روی تمام تنظیمات ماسک و فرمت اولویت دارد
            if (e.Value is DateTime dt && dt != DateTime.MinValue && dt != DateTime.MaxValue)
            {
                try
                {
                    // استفاده از کلاس PersianDate برای تولید رشته شمسی
                    // نکته: اگر فرمت خاصی مد نظر است (مثلا yyyy/MM/dd) اینجا تغییر دهید
                    e.DisplayText = new PersianDate(dt).ToString("d");
                }
                catch
                {
                    // در صورت بروز هرگونه خطا، به نمایش پیش‌فرض میلادی بسنده کن تا برنامه کرش نکند
                }
            }
        }

        // ۴. بازنویسی GetDisplayText برای حالت‌های غیر فوکوس و گرید
        public override string GetDisplayText(FormatInfo format, object editValue)
        {
            if (editValue is DateTime dt && dt != DateTime.MinValue && dt != DateTime.MaxValue)
            {
                return new PersianDate(dt).ToString("d");
            }
            return base.GetDisplayText(format, editValue);
        }
    }

    public class PersianDateEditValueConverter : DateEditValueConverter
    {
        private readonly IDateTimeOwner owner;

        public PersianDateEditValueConverter(IDateTimeOwner owner) : base(owner)
        {
            this.owner = owner;
        }

        public new DateTime ConvertToDateTime(object val)
        {
            var converted = ConvertToObject(owner.DoParseEditValue(val));
            if (converted is DateTime)
                return (DateTime)converted;

            var editValueEventArgs = owner.DoFormatEditValue(converted);
            if (editValueEventArgs.Value is DateTime)
                return (DateTime)editValueEventArgs.Value;

            if (owner.NullDate is DateTime)
                return (DateTime)owner.NullDate;

            return PersianDate.MinValue;
        }

        protected override object ConvertToObject(ConvertEditValueEventArgs args)
        {
            var obj = args.Value;
            if (args.Handled)
                return obj;

            if (obj == null || obj == DBNull.Value)
                return null;

            if (obj.Equals(owner.NullDate))
                return null;

            if (obj is string && ((string)obj).Length == 0)
                return null;

            if (obj is DateTime)
            {
                var dt = (DateTime)obj;
                if (!PersianCalendar.IsWithInSupportedRange(dt))
                    return null;

                return dt;
            }

            try
            {
                // تلاش برای پارس کردن تاریخ
                DateTime result;
                // اینجا بهتر است ابتدا سعی کنیم شمسی پارس کنیم
                if (PersianDate.TryParse(obj.ToString(), out PersianDate pd))
                {
                    return pd.ToDateTime();
                }

                if (DateTime.TryParse(obj.ToString(), out result) &&
                    PersianCalendar.IsWithInSupportedRange(result))
                {
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    public class PersianDateEditFormatInfo : FormatInfo
    {
        private const string format = "yyyy/MM/dd";

        public PersianDateEditFormatInfo(IComponentLoading componentLoading) : base(componentLoading)
        {
            FormatType = FormatType.DateTime;
            FormatString = format;
        }

        public PersianDateEditFormatInfo()
        {
            FormatType = FormatType.DateTime;
            FormatString = format;
        }

        protected override void ResetFormatType()
        {
            FormatType = FormatType.DateTime;
        }

        public override bool ShouldSerialize()
        {
            return FormatType != FormatType.DateTime;
        }

        protected override bool ShouldSerializeFormatString()
        {
            return FormatString != format;
        }

        protected override void ResetFormatString()
        {
            FormatString = format;
        }
    }


    public class PersianDateTimeMaskManager : TextMaskManager
    {
    }
}