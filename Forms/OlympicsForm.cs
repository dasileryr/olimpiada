using Olimpiada.DataAccess;
using Olimpiada.Models;
using System.Data;

namespace Olimpiada.Forms
{
    public partial class OlympicsForm : Form
    {
        private DatabaseHelper dbHelper;
        private DataGridView dataGridView;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;

        public OlympicsForm(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();

            // dataGridView
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 12);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(760, 400);
            dataGridView.TabIndex = 0;

            // btnAdd
            btnAdd.Location = new Point(12, 430);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 30);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += BtnAdd_Click;

            // btnEdit
            btnEdit.Location = new Point(118, 430);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Редактировать";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;

            // btnDelete
            btnDelete.Location = new Point(224, 430);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;

            // btnClose
            btnClose.Location = new Point(672, 430);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 4;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            // OlympicsForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 472);
            Controls.Add(btnClose);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(dataGridView);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OlympicsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Олимпиады";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        private void LoadData()
        {
            var olympics = dbHelper.GetAllOlympics();
            dataGridView.DataSource = olympics.Select(o => new
            {
                o.OlympicsId,
                Год = o.Year,
                Тип = o.IsSummer ? "Летняя" : "Зимняя",
                Страна = o.HostCountryName,
                Город = o.City
            }).ToList();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new OlympicsEditForm(dbHelper, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите олимпиаду для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var olympicsId = (int)dataGridView.SelectedRows[0].Cells[0].Value;
            var olympics = dbHelper.GetAllOlympics().FirstOrDefault(o => o.OlympicsId == olympicsId);
            if (olympics != null)
            {
                var form = new OlympicsEditForm(dbHelper, olympics);
                if (form.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите олимпиаду для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить эту олимпиаду?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var olympicsId = (int)dataGridView.SelectedRows[0].Cells[0].Value;
                dbHelper.DeleteOlympics(olympicsId);
                LoadData();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

