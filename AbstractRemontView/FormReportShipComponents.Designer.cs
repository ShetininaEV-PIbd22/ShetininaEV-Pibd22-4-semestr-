namespace AbstractRemontView
{
    partial class FormReportShipComponents
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonToPdf = new System.Windows.Forms.Button();
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // buttonToPdf
            // 
            this.buttonToPdf.Location = new System.Drawing.Point(606, 1);
            this.buttonToPdf.Name = "buttonToPdf";
            this.buttonToPdf.Size = new System.Drawing.Size(75, 23);
            this.buttonToPdf.TabIndex = 5;
            this.buttonToPdf.Text = "В pdf";
            this.buttonToPdf.UseVisualStyleBackColor = true;
            this.buttonToPdf.Click += new System.EventHandler(this.ButtonToPdf_Click);
            // 
            // reportViewer
            // 
            this.reportViewer.LocalReport.ReportEmbeddedResource = "AbstractRemontView.Report.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(5, 30);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new System.Drawing.Size(676, 318);
            this.reportViewer.TabIndex = 6;
            this.reportViewer.Load += new System.EventHandler(this.reportViewer_Load);
            // 
            // FormReportShipComponents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 350);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.buttonToPdf);
            this.Name = "FormReportShipComponents";
            this.Text = "Компоненты по кораблям";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonToPdf;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
    }
}

