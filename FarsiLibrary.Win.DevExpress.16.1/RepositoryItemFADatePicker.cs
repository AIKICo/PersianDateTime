using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using FarsiLibrary.Utils;
using FarsiLibrary.Utils.Internals;

namespace FarsiLibrary.Win.DevExpress
{
    [UserRepositoryItem("Register")]
    public class RepositoryItemFADatePicker : RepositoryItemPopupContainerEdit
    {
        public const string EditorName = "XtraFADatePicker";

        #region Ctor

        static RepositoryItemFADatePicker()
        {
            Register();
        }

        public RepositoryItemFADatePicker()
        {
            TextEditStyle = TextEditStyles.Standard;
            CloseUpKey = new KeyShortcut(Keys.F4);
            PopupSizeable = false;
            ShowPopupCloseButton = false;
        }

        #endregion

        #region Registration

        /// <summary>
        /// Registers the repository
        /// </summary>
        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(XtraFADatePicker), typeof(RepositoryItemFADatePicker), typeof(PopupContainerEditViewInfo), new ButtonEditPainter(), true, EditImageIndexes.DateEdit));
        }

        #endregion

        #region Properties

        public bool ShouldSerializeNullDateCalendarValue()
        {
            return false;
        }

        /// <summary>
        /// Properties
        /// </summary>
        public new RepositoryItemFADatePicker Properties
        {
            get { return this; }
        }

        /// <summary>
        /// EditorTypeName
        /// </summary>
        [Browsable(false)]
        public override string EditorTypeName
        {
            get { return EditorName; }
        }

        /// <summary>
        /// Gets the owner edit.
        /// </summary>
        /// <value>The owner edit.</value>
        protected new XtraFADatePicker OwnerEdit
        {
            get { return base.OwnerEdit as XtraFADatePicker; }
        }

        #endregion

        #region Methods

        protected internal void SetOwner(XtraFADatePicker owner)
        {
            SetOwnerEdit(owner);
        }

        /// <summary>
        /// Assigns the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Assign(RepositoryItem item)
        {
            RepositoryItemFADatePicker source = item as RepositoryItemFADatePicker;
            BeginUpdate();
            try
            {
                PopupContainerControl popupControl = PopupControl;
                base.Assign(item);
                PopupControl = popupControl;

                if (source == null)
                    return;

                LookAndFeel.Assign(source.LookAndFeel);
            }
            finally
            {
                EndUpdate();
            }
        }

        protected override void RaiseQueryPopUp(CancelEventArgs e)
        {
            base.RaiseQueryPopUp(e);

            if (PopupControl != null)
            {
                PopupControl.Size = new Size(OwnerEdit.DEFAULT_SIZE, OwnerEdit.DEFAULT_SIZE);
                PopupControl.PopupContainerProperties.PopupFormWidth = OwnerEdit.DEFAULT_SIZE;
            }

            if (OwnerEdit != null)
            {
                OwnerEdit.UpdateTheme();
            }
        }

        protected override void RaiseQueryDisplayText(QueryDisplayTextEventArgs e)
        {
            // فراخوانی متد پایه برای اطمینان از صحت عملکرد
            base.RaiseQueryDisplayText(e);

            if (e.EditValue != null && e.EditValue is DateTime)
            {
                DateTime dt = (DateTime)e.EditValue;

                // شرط IsFarsiCulture حذف شد. همیشه تبدیل انجام می‌شود.
                // بررسی محدوده معتبر برای جلوگیری از خطا
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                {
                    try
                    {
                        PersianDate pd = new PersianDate(dt);
                        e.DisplayText = FormatDisplayText(pd);
                    }
                    catch
                    {
                        // در صورت بروز خطا، همان تاریخ میلادی را نشان دهد
                        e.DisplayText = FormatDisplayText(dt);
                    }
                }

                if (OwnerEdit != null)
                    OwnerEdit.MonthView.SelectedDateTime = dt;
            }

            if (e.EditValue == null)
                e.DisplayText = Properties.NullText;

            if (OwnerEdit != null)
            {
                OwnerEdit.ToolTip = (!string.IsNullOrEmpty(e.DisplayText) && OwnerEdit.ShowToolTips) ? e.DisplayText : null;
            }
        }

        public override string GetDisplayText(object editValue)
        {
            if (editValue is DateTime)
            {
                DateTime dt = (DateTime)editValue;
                // شرط IsFarsiCulture حذف شد
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                {
                    return FormatDisplayText(new PersianDate(dt));
                }
                return FormatDisplayText(dt);
            }
            return string.Empty;
        }

        protected virtual string FormatDisplayText(object value)
        {
            if (value is PersianDate)
            {
                return ((PersianDate)value).ToString("d");
            }

            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                    return Properties.NullText;

                // تبدیل اجباری به شمسی
                return new PersianDate(dt).ToString("d");
            }

            return Properties.NullText;
        }

        #endregion
    }
}