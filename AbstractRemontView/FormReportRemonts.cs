using AbstractRemontBusinessLogic.BusinessLogics;
using AbstractRemontBusinessLogic.BindingModels;
using System;
using System.Windows.Forms;
using Unity;
using System.Linq;
using System.Collections.Generic;

namespace AbstractRemontView
{
    public partial class FormReportRemonts : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ReportLogic logic;
        public FormReportRemonts(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void ButtonSaveToExcel_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Date to " + dateTimePickerTo.Value.Date);
            Console.WriteLine("Date from " + dateTimePickerFrom.Value.Date);
            if (dateTimePickerTo.Value.Date>=dateTimePickerFrom.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveOrdersToExcelFile(new ReportBindingModel { FileName = dialog.FileName, DateFrom = dateTimePickerFrom.Value.Date, DateTo = dateTimePickerTo.Value.Date });

                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ButtonMake_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Date to " + dateTimePickerTo.Value.Date);
            Console.WriteLine("Date from " + dateTimePickerFrom.Value.Date);
            if (dateTimePickerTo.Value.Date >= dateTimePickerFrom.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var dict = logic.GetOrders(new ReportBindingModel { DateFrom = dateTimePickerFrom.Value.Date, DateTo = dateTimePickerTo.Value.Date });
                List<DateTime> dates = new List<DateTime>();
                foreach (var order in dict)
                {
                    if (!dates.Contains(order.DateCreate.Date))
                    {
                        dates.Add(order.DateCreate.Date);
                    }
                }

                if (dict != null)
                {
                    dataGridView.Rows.Clear();

                    foreach (var date in dates)
                    {
                        decimal dateSum = 0;

                        dataGridView.Rows.Add(new object[] { date.Date, "", "" });

                        foreach (var order in dict.Where(rec => rec.DateCreate.Date == date.Date))
                        {
                            dataGridView.Rows.Add(new object[] { "", order.ShipName, order.Sum });
                            dateSum += order.Sum;
                        }

                        dataGridView.Rows.Add(new object[] { "Итого", "", dateSum });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

