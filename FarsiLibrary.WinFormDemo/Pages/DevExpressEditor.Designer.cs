using DevExpress.XtraEditors;
using FarsiLibrary.Win.DevExpress;

namespace FarsiLibrary.WinFormDemo.Pages
{
    partial class DevExpressEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevExpressEditor));
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lblDatePickerValue = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            lblDateEditValue = new System.Windows.Forms.Label();
            lblTouchUIValue = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            xtraFADateEdit1 = new XtraFADateEdit();
            xtraFADatePicker1 = new XtraFADatePicker();
            dateEdit1 = new XtraFADateEdit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            label1.Size = new System.Drawing.Size(862, 81);
            label1.TabIndex = 1;
            label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(104, 81);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(134, 27);
            label2.TabIndex = 2;
            label2.Text = "Date Picker : ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(161, 107);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(77, 27);
            label3.TabIndex = 3;
            label3.Text = "EditValue : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDatePickerValue
            // 
            lblDatePickerValue.Location = new System.Drawing.Point(245, 107);
            lblDatePickerValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDatePickerValue.Name = "lblDatePickerValue";
            lblDatePickerValue.Size = new System.Drawing.Size(236, 27);
            lblDatePickerValue.TabIndex = 4;
            lblDatePickerValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(77, 155);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(161, 27);
            label4.TabIndex = 6;
            label4.Text = "DevExpress Date Picker : ";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(161, 181);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(77, 27);
            label5.TabIndex = 7;
            label5.Text = "EditValue : ";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDateEditValue
            // 
            lblDateEditValue.Location = new System.Drawing.Point(245, 181);
            lblDateEditValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDateEditValue.Name = "lblDateEditValue";
            lblDateEditValue.Size = new System.Drawing.Size(236, 27);
            lblDateEditValue.TabIndex = 8;
            lblDateEditValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTouchUIValue
            // 
            lblTouchUIValue.Location = new System.Drawing.Point(245, 260);
            lblTouchUIValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTouchUIValue.Name = "lblTouchUIValue";
            lblTouchUIValue.Size = new System.Drawing.Size(236, 27);
            lblTouchUIValue.TabIndex = 12;
            lblTouchUIValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(161, 260);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(77, 27);
            label8.TabIndex = 11;
            label8.Text = "EditValue : ";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(77, 233);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(161, 27);
            label6.TabIndex = 10;
            label6.Text = "DevExpress TouchUI : ";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // xtraFADateEdit1
            // 
            xtraFADateEdit1.EditValue = null;
            xtraFADateEdit1.Location = new System.Drawing.Point(245, 235);
            xtraFADateEdit1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            xtraFADateEdit1.Name = "xtraFADateEdit1";
            xtraFADateEdit1.Size = new System.Drawing.Size(236, 20);
            xtraFADateEdit1.TabIndex = 9;
            xtraFADateEdit1.EditValueChanged += xtraFADateEdit1_EditValueChanged;
            // 
            // xtraFADatePicker1
            // 
            xtraFADatePicker1.Location = new System.Drawing.Point(245, 83);
            xtraFADatePicker1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            xtraFADatePicker1.Name = "xtraFADatePicker1";
            xtraFADatePicker1.Size = new System.Drawing.Size(236, 20);
            xtraFADatePicker1.TabIndex = 0;
            xtraFADatePicker1.EditValueChanged += xtraFADatePicker1_EditValueChanged;
            // 
            // dateEdit1
            // 
            dateEdit1.EditValue = null;
            dateEdit1.Location = new System.Drawing.Point(245, 157);
            dateEdit1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dateEdit1.Name = "dateEdit1";
            dateEdit1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            dateEdit1.Size = new System.Drawing.Size(236, 20);
            dateEdit1.TabIndex = 5;
            dateEdit1.UpdateSelectionWhenNavigating = true;
            dateEdit1.EditValueChanged += dateEdit1_EditValueChanged;
            // 
            // DevExpressEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblTouchUIValue);
            Controls.Add(label8);
            Controls.Add(label1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(xtraFADateEdit1);
            Controls.Add(dateEdit1);
            Controls.Add(xtraFADatePicker1);
            Controls.Add(lblDatePickerValue);
            Controls.Add(lblDateEditValue);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label3);
            IsNew = true;
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "DevExpressEditor";
            Size = new System.Drawing.Size(862, 592);
            Title = "DevExpress Custom Editor";
            ResumeLayout(false);

        }

        #endregion

        private XtraFADatePicker xtraFADatePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDatePickerValue;
        private XtraFADateEdit dateEdit1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDateEditValue;
        private System.Windows.Forms.Label lblTouchUIValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private XtraFADateEdit xtraFADateEdit1;
    }
}
