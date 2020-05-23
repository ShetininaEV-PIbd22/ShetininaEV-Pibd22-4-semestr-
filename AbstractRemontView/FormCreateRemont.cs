using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.BusinessLogics;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace AbstractRemontView
{
    public partial class FormCreateRemont : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IShipLogic logicP;
        private readonly IClientLogic logicC;
        private readonly MainLogic logicM;

        public FormCreateRemont(IShipLogic logicP, IClientLogic logicC, MainLogic logicM)
        {
            InitializeComponent();
            this.logicP = logicP;
            this.logicC = logicC;
            this.logicM = logicM;
        }

        private void FormCreateRemont_Load(object sender, EventArgs e)
        {
            try
            { //  Логика загрузки списка компонент в выпадающий список
                List<ShipViewModel> list = logicP.Read(null);
                if (list != null)
                {
                    comboBoxShip.DisplayMember = "ShipName";
                    comboBoxShip.ValueMember = "Id";
                    comboBoxShip.DataSource = list;
                    comboBoxShip.SelectedItem = null;
                }
                List<ClientViewModel> listC = logicC.Read(null);
                if (listC != null)
                {
                    comboBoxClient.DisplayMember = "FIO";
                    comboBoxClient.ValueMember = "Id";
                    comboBoxClient.DataSource = listC;
                    comboBoxClient.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxShip.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxShip.SelectedValue);
                    ShipViewModel product = logicP.Read(new ShipBindingModel { Id = id })?[0];
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product?.Price ?? 0).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxShip.SelectedValue == null)
            {
                MessageBox.Show("Выберите корабль", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxClient.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                logicM.CreateRemont(new CreateRemontBindingModel
                {
                    ShipId = Convert.ToInt32(comboBoxShip.SelectedValue),
                    ClientFIO = (comboBoxClient.SelectedItem as ClientViewModel).FIO,
                    ClientId = Convert.ToInt32(comboBoxClient.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
