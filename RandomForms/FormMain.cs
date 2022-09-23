namespace RandomForms;

public partial class FormMain : Form {
    private List<int>? ints;

    public FormMain() => InitializeComponent();

    private void button1_Click(object sender, EventArgs e) {
        var minValue = int.Parse(startBox.Text);
        var maxValue = int.Parse(endBox.Text) + 1;

        if (minValue < 0) ErrMsg("최소값은 음수일 수 없습니다.");

        if (!checkBox1.Checked) {
            add(getRandom());
        } else {
            if (ints == null || (ints.Capacity == (maxValue - minValue) && ints.Count >= (maxValue - minValue)) || ints.Capacity != (maxValue - minValue)) {
                listBox1.Items.Clear();
                ints = new(maxValue - minValue);
                add(getRandom());
            } else {
                int value;

                do {
                    value = getRandom();
                } while (ints.Contains(value));

                add(value);
            }
        }

        int getRandom() => Random.Shared.Next(minValue, maxValue);

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
}