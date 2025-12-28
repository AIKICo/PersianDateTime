using System;
using System.ComponentModel;
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

namespace FarsiLibrary.Win.DevExpress
{
    // =================================================================================
    // کلاس اصلی کنترل (XtraFADateEdit)
    // =================================================================================
    public class XtraFADateEdit : DateEdit
    {
        public const string EditorName = "XtraFADateEdit";

        static XtraFADateEdit()
        {
            Register();
        }

        public XtraFADateEdit()
        {
            // تنظیمات پیش‌فرض در سازنده
        }

        public override string EditorTypeName => EditorName;

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
                EditorName,
                typeof(XtraFADateEdit),
                typeof(RepositoryItemXtraFADateEdit),
                typeof(DateEditViewInfo),
                new ButtonEditPainter(),
                true,
                EditImageIndexes.DateEdit));
        }

        protected override PopupBaseForm CreatePopupForm()
        {
            // اجبار به استفاده از پاپ‌آپ شمسی (بدون شرط زبان سیستم)
            if (Properties.CalendarView == CalendarView.TouchUI)
                return new FATouchPopupDateEditForm(this);

            return new VistaPopupPersianDateEditForm(this);
        }
    }

    // =================================================================================
    // کلاس تنظیمات مخزن (RepositoryItem)
    // =================================================================================
    [UserRepositoryItem("Register")]
    public class RepositoryItemXtraFADateEdit : RepositoryItemDateEdit
    {
        // -------------------------------------------------------------------------
        // ۱. تعریف خاصیت EnsureDefaultButton (حل خطای کامپایل شما)
        // -------------------------------------------------------------------------
        private bool _ensureDefaultButton = true;

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Determines whether the default dropdown button is automatically added.")]
        public bool EnsureDefaultButton
        {
            get { return _ensureDefaultButton; }
            set { _ensureDefaultButton = value; }
        }

        static RepositoryItemXtraFADateEdit()
        {
            Register();
        }

        public RepositoryItemXtraFADateEdit()
        {
            // ۲. تنظیمات اولیه برای رفع مشکل نمایش میلادی
            this.UseMaskAsDisplayFormat = false; // غیرفعال کردن ماسک برای نمایش متن

            // تنظیم ماسک برای حالت ویرایش (فوکوس)
            // استفاده از RegEx باعث می‌شود کنترلر سعی نکند تاریخ را به میلادی پارس کند
            this.Mask.MaskType = MaskType.RegEx;
            this.Mask.EditMask = @"[1-9][0-9]{3}/[0-1]?[0-9]/[0-3]?[0-9]";
            this.Mask.UseMaskAsDisplayFormat = false;
        }

        public const string EditorName = "XtraFADateEdit";

        public override string EditorTypeName => EditorName;

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(
                EditorName,
                typeof(XtraFADateEdit),
                typeof(RepositoryItemXtraFADateEdit),
                typeof(DateEditViewInfo),
                new ButtonEditPainter(),
                true,
                EditImageIndexes.DateEdit));
        }

        // -------------------------------------------------------------------------
        // ۳. منطق اضافه کردن دکمه پیش‌فرض (مربوط به EnsureDefaultButton)
        // -------------------------------------------------------------------------
        public override void EndInit()
        {
            base.EndInit();
            if (EnsureDefaultButton && Buttons.Count == 0)
            {
                CreateDefaultButton();
            }
        }

        public override void CreateDefaultButton()
        {
            // ساخت دکمه بازشو استاندارد
            EditorButton button = new EditorButton(ButtonPredefines.Combo);
            button.IsDefaultButton = true;
            Buttons.Add(button);
        }

        // -------------------------------------------------------------------------
        // ۴. بازنویسی رویداد تبدیل متن برای نمایش همیشه شمسی
        // -------------------------------------------------------------------------
        protected override void RaiseCustomDisplayText(CustomDisplayTextEventArgs e)
        {
            base.RaiseCustomDisplayText(e);

            // تبدیل مقدار EditValue (که میلادی است) به متن شمسی برای نمایش
            if (e.Value is DateTime dt && dt != DateTime.MinValue && dt != DateTime.MaxValue)
            {
                try
                {
                    e.DisplayText = new PersianDate(dt).ToString("d"); // فرمت مثال: 1403/10/08
                }
                catch
                {
                    // در صورت خطا، متن تغییر نکند
                }
            }
        }

        public override string GetDisplayText(FormatInfo format, object editValue)
        {
            if (editValue is DateTime dt && dt != DateTime.MinValue && dt != DateTime.MaxValue)
            {
                return new PersianDate(dt).ToString("d");
            }
            return base.GetDisplayText(format, editValue);
        }

        // -------------------------------------------------------------------------
        // ۵. تنظیم کانورتر اختصاصی برای تبدیل رشته تایپ شده به تاریخ میلادی
        // -------------------------------------------------------------------------
        protected override DateEditValueConverter CreateConverter()
        {
            return new PersianDateEditValueConverter(this);
        }
    }

    // =================================================================================
    // کلاس مبدل داده (Converter)
    // مسئول تبدیل متن فارسی وارد شده به تاریخ میلادی پشت صحنه
    // =================================================================================
    public class PersianDateEditValueConverter : DateEditValueConverter
    {
        public PersianDateEditValueConverter(IDateTimeOwner owner) : base(owner) { }

        protected override object ConvertToObject(ConvertEditValueEventArgs args)
        {
            object val = args.Value;

            if (val == null || val == DBNull.Value)
                return null;

            // اگر ورودی رشته است (تایپ کاربر یا متن ماسک)
            if (val is string strVal)
            {
                strVal = strVal.Trim();
                if (string.IsNullOrEmpty(strVal)) return null;

                // تلاش برای تبدیل رشته شمسی به میلادی
                if (PersianDate.TryParse(strVal, out PersianDate pd))
                {
                    return pd.ToDateTime();
                }

                // پشتیبانی از فرمت میلادی (اگر کاربر میلادی وارد کرد)
                if (DateTime.TryParse(strVal, out DateTime dt))
                {
                    return dt;
                }
            }

            return base.ConvertToObject(args);
        }
    }
}