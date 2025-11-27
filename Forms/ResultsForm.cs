using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class ResultsForm : Form
    {
        private DatabaseHelper dbHelper;
        private DataGridView dataGridView;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnClose;

        public ResultsForm(DatabaseHelper dbHelper)
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

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 12);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(960, 400);
            dataGridView.TabIndex = 0;

            btnAdd.Location = new Point(12, 430);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 30);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += BtnAdd_Click;

            btnEdit.Location = new Point(118, 430);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Редактировать";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;

            btnDelete.Location = new Point(224, 430);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;

            btnClose.Location = new Point(872, 430);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 4;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 472);
            Controls.Add(btnClose);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(dataGridView);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ResultsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Результаты";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        private void LoadData()
        {
            var results = dbHelper.GetAllResults();
            var olympics = dbHelper.GetAllOlympics();
            dataGridView.DataSource = results.Select(r => new
            {
                r.ResultId,
                Олимпиада = $"{olympics.FirstOrDefault(o => o.OlympicsId == r.OlympicsId)?.Year} - {olympics.FirstOrDefault(o => o.OlympicsId == r.OlympicsId)?.City}",
                Вид_спорта = r.SportName,
                Спортсмен = r.AthleteName,
                Медаль = r.MedalName
            }).ToList();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new ResultEditForm(dbHelper, null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите результат для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultId = (int)dataGridView.SelectedRows[0].Cells[0].Value;
            var result = dbHelper.GetAllResults().FirstOrDefault(r => r.ResultId == resultId);
            if (result != null)
            {
                var form = new ResultEditForm(dbHelper, result);
                if (form.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите результат для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить этот результат?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var resultId = (int)dataGridView.SelectedRows[0].Cells[0].Value;
                dbHelper.DeleteResult(resultId);
                LoadData();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

