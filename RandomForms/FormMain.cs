namespace RandomForms;

public partial class FormMain : Form {
    private List<int>? ints;

    public FormMain() => InitializeComponent();

    private void button1_Click(object sender, EventArgs e) {
        if (!int.TryParse(startBox.Text, out var minValue) || !int.TryParse(endBox.Text, out var maxValue)) {
            ErrMsg("숫자가 아닌 문자열을 입력했거나 숫자가 너무 큽니다.");
            return;
        }

        if (minValue < 0) {
            ErrMsg("최소값은 음수일 수 없습니다.");
            return;
        }

        if (minValue > maxValue) {
            ErrMsg("최소값은 최대값보다 클 수 없습니다.");
            return;
        }

        maxValue++;

        if (!checkBox1.Checked) {
            ints = null;
            add(Random.Shared.Next(minValue, maxValue));
        } else {
            if (ints == null || ints.Capacity != (maxValue - minValue) || ints.Count >= (maxValue - minValue)) {
                listBox1.Items.Clear();
                ints = new(maxValue - minValue);
                add(Random.Shared.Next(minValue, maxValue));
            } else {
                int value;

                do {
                    value = Random.Shared.Next(minValue, maxValue);
                } while (ints.Contains(value));

                add(value);
            }
        }

        void add(int value) {
            numlabel.Text = value.ToString();
            listBox1.Items.Add(value);
            if (ints != null) ints.Add(value);
        }
    }

    private void button2_Click(object sender, EventArgs e) {
        numlabel.Text = null;
        listBox1.Items.Clear();
        if (ints != null) ints = null;
    }

    private void textBox_TextChanged(object sender, EventArgs e) => button1.Enabled = !string.IsNullOrWhiteSpace(startBox.Text) && !string.IsNullOrWhiteSpace(endBox.Text);
}