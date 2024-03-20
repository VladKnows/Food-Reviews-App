using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOOD_REVIEWS_INTERFACE
{
    public partial class Search : Form
    {
        private void emptyComboBoxes(List<ComboBox> comboBoxes)
        {
            for(int i = 0; i < comboBoxes.Count; ++i)
            {
                comboBoxes[i].Items.Clear();
            }
        }

        private void emptyDataGridViews(List<DataGridView> dataGridViews)
        {
            for (int i = 0; i < dataGridViews.Count; ++i)
            {
                dataGridViews[i].Rows.Clear();
                dataGridViews[i].Columns.Clear();
            }
        }

        private void putInDataGrid(List<List<string>> data, DataGridView dg)
        {
            for(int i = 0; i < data[0].Count; ++i)
                dg.Columns.Add(data[0][i], data[0][i]);
            for (int i = 1; i < data.Count; ++i)
            {
                string[] row = data[i].ToArray();
                dg.Rows.Add(row);
            }
        }

        private void putInSelectDataGrid(DataGridView dg)
        {
            bool[] wheres = new bool[5] { false, false, false, false, false};
            if (ch_WhereSelectionFoodCategory.Checked)
                wheres[0] = true;
            if (ch_WhereSelectionDish.Checked)
                wheres[1] = true;
            if (ch_WhereSelectionPrice.Checked)
                wheres[2] = true;
            if (ch_WhereSelectionWeight.Checked)
                wheres[3] = true;
            if (ch_WhereSelectionScore.Checked)
                wheres[4] = true;

            Control[] controls = new Control[5] { cb_WhereSelectionFoodCategory, cb_WhereSelectionDish, num_WhereSelectionPrice, num_WhereSelectionWeight, num_WhereSelectionScore };
            List<List<string>> table = OracleDB.runBigSelectCommand(dg, wheres, controls);
            putInDataGrid(table, dg);
        }

        private void initializeComboBoxes()
        {
            List<ComboBox> comboBoxes = new List<ComboBox> { cb_WhereUserID, cb_WhereUserName,
                cb_WhereUserPhone, cb_WhereReviewID, cb_WhereFoodCategoryID,
                cb_WhereFoodCategoryName, cb_WhereDishID, cb_WhereDishName,
                cb_WhereManagerID, cb_WhereManagerName, cb_WhereManagerPhone,
                cb_WhereManagerMailAddress, cb_WhereRestaurantID, cb_WhereRestaurantName,
                cb_WhereRestaurantManagerID, cb_WhereRestaurantDishID, cb_ReviewUserID,
                cb_ReviewRestaurantDishID, cb_DishFoodCategoryID, cb_RestaurantManagerID,
                cb_RestaurantDishesDishID, cb_RestaurantDishesRestaurantID, cb_WhereSelectionDish,
                cb_WhereSelectionFoodCategory
            };
            emptyComboBoxes(comboBoxes);
            List<DataGridView> dgws = new List<DataGridView>
            {
                dg_Dish, dg_FoodCategory, dg_Manager, dg_Restaurant, dg_RestaurantDish,
                dg_Review, dg_User, dg_Selection
            };
            emptyDataGridViews(dgws);

            OracleDB.getChoices(cb_WhereUserID, "USERS", "ID_USER");
            OracleDB.getChoices(cb_WhereUserName, "USERS", "NAME");
            OracleDB.getChoices(cb_WhereUserPhone, "USERS", "PHONE_NUMBER");

            OracleDB.getChoices(cb_WhereReviewID, "REVIEWS", "ID_REVIEW");

            OracleDB.getChoices(cb_WhereFoodCategoryID, "FOOD_CATEGORIES", "ID_FOOD_CATEGORY");
            OracleDB.getChoices(cb_WhereFoodCategoryName, "FOOD_CATEGORIES", "NAME");

            OracleDB.getChoices(cb_WhereDishID, "DISHES", "ID_DISH");
            OracleDB.getChoices(cb_WhereDishName, "DISHES", "NAME");

            OracleDB.getChoices(cb_WhereManagerID, "MANAGERS", "ID_MANAGER");
            OracleDB.getChoices(cb_WhereManagerName, "MANAGERS", "NAME");
            OracleDB.getChoices(cb_WhereManagerPhone, "MANAGERS", "CONTACT_NUMBER");
            OracleDB.getChoices(cb_WhereManagerMailAddress, "MANAGERS", "MAIL_ADDRESS");

            OracleDB.getChoices(cb_WhereRestaurantID, "RESTAURANTS", "ID_RESTAURANT");
            OracleDB.getChoices(cb_WhereRestaurantName, "RESTAURANTS", "NAME");
            OracleDB.getChoices(cb_WhereRestaurantManagerID, "RESTAURANTS", "ID_MANAGER");

            OracleDB.getChoices(cb_WhereRestaurantDishID, "RESTAURANT_DISHES", "ID_RESTAURANT_DISHES");

            OracleDB.getChoices(cb_ReviewUserID, "USERS", "ID_USER");
            OracleDB.getChoices(cb_ReviewRestaurantDishID, "RESTAURANT_DISHES", "ID_RESTAURANT_DISHES");

            OracleDB.getChoices(cb_DishFoodCategoryID, "FOOD_CATEGORIES", "ID_FOOD_CATEGORY");

            OracleDB.getChoices(cb_RestaurantManagerID, "MANAGERS", "ID_MANAGER");

            OracleDB.getChoices(cb_RestaurantDishesDishID, "DISHES", "ID_DISH");
            OracleDB.getChoices(cb_RestaurantDishesRestaurantID, "RESTAURANTS", "ID_RESTAURANT");

            OracleDB.getChoices(cb_WhereSelectionFoodCategory, "FOOD_CATEGORIES", "NAME");
            OracleDB.getChoices(cb_WhereSelectionDish, "DISHES", "NAME");

            List<string> tables = new List<string> { "USERS", "DISHES", "RESTAURANTS", "FOOD_CATEGORIES", "MANAGERS", "RESTAURANT_DISHES", "REVIEWS" };
            List<DataGridView> dgs = new List<DataGridView> { dg_User, dg_Dish, dg_Restaurant, dg_FoodCategory, dg_Manager, dg_RestaurantDish, dg_Review };
            for(int i = 0; i < tables.Count; ++i)
            {
                List<List<string>> table = OracleDB.runSelectAllCommand(tables[i]);
                putInDataGrid(table, dgs[i]);
            }

            putInSelectDataGrid(dg_Selection);
        }

        public Search()
        {
            OracleDB.connectToDataBase(); 
            InitializeComponent();

            initializeComboBoxes();
        }

        private void resetControls(List<Control> controls)
        {
            for (int i = 0; i < controls.Count; ++i)
            {
                if (controls[i].GetType() == typeof(TextBox))
                {
                    TextBox tb = (TextBox)controls[i];
                    tb.Text = "";
                }
                else if (controls[i].GetType() == typeof(NumericUpDown))
                {
                    NumericUpDown num = (NumericUpDown)controls[i];
                    num.Value = 1;
                }
                else if (controls[i].GetType() == typeof(ComboBox))
                {
                    ComboBox cb = (ComboBox)controls[i];
                    cb.Text = "";
                }
            }
        }

        private void bt_ReviewAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_REVIEW", "ID_USER", "ID_RESTAURANT_DISHES", "SCORE", "DESCRIPTION"};
            List<string> values = new List<string> { "REVIEWS_ID_REVIEW_SEQ.NEXTVAL", $"'{cb_ReviewUserID.Text}'", $"'{cb_ReviewRestaurantDishID.Text}'", $"'{num_ReviewScore.Value.ToString()}'", $"'{tb_ReviewDescription.Text}'" };

            if (OracleDB.runTransactionCommand("REVIEWS", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control>{ cb_ReviewUserID, cb_ReviewRestaurantDishID, num_ReviewScore, tb_ReviewDescription };
                resetControls(controls);
            }
        }

        private void bt_UserAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_USER", "NAME", "PHONE_NUMBER" };
            List<string> values = new List<string> { "USERS_ID_USER_SEQ.NEXTVAL", $"'{tb_UserName.Text}'", $"'{tb_UserPhone.Text}'" };

            if (OracleDB.runInsertCommand("USERS", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control> { tb_UserName, tb_UserPhone };
                resetControls(controls);
            }
        }

        private void bt_FoodCategoryAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_FOOD_CATEGORY", "NAME", "DESCRIPTION" };
            List<string> values = new List<string> { "FOOD_CATEGORIES_ID_FOOD_CATEGO.NEXTVAL", $"'{tb_FoodCategoryName.Text}'", $"'{tb_FoodCategoryDescription.Text}'" };

            if (OracleDB.runInsertCommand("FOOD_CATEGORIES", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control> { tb_FoodCategoryName, tb_FoodCategoryDescription };
                resetControls(controls);
            }
        }

        private void bt_DishAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_DISH", "ID_FOOD_CATEGORY", "NAME", "INGREDIENTS" };
            List<string> values = new List<string> { "DISHES_ID_DISH_SEQ.NEXTVAL", $"'{cb_DishFoodCategoryID.Text}'", $"'{tb_DishName.Text}'", $"'{tb_DishIngredients.Text}'" };

            if (OracleDB.runInsertCommand("DISHES", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control> { cb_DishFoodCategoryID, tb_DishName, tb_DishIngredients };
                resetControls(controls);
            }
        }

        private void bt_ManagerAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_MANAGER", "NAME", "CONTACT_NUMBER", "MAIL_ADDRESS" };
            List<string> values = new List<string> { "MANAGERS_ID_MANAGER_SEQ.NEXTVAL", $"'{tb_ManagerName.Text}'", $"'{tb_ManagerPhone.Text}'", $"'{tb_ManagerMailAddress.Text}'" };

            if (OracleDB.runInsertCommand("MANAGERS", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control> { tb_ManagerName, tb_ManagerPhone, tb_ManagerMailAddress };
                resetControls(controls);
            }
        }

        private void bt_RestaurantAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_RESTAURANT", "ID_MANAGER", "NAME", "ADDRESS" };
            List<string> values = new List<string> { "RESTAURANTS_ID_RESTAURANT_SEQ.NEXTVAL", $"'{cb_RestaurantManagerID.Text}'", $"'{tb_RestaurantName.Text}'", $"'{tb_RestaurantAddress.Text}'" };

            if (OracleDB.runInsertCommand("RESTAURANTS", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control> { cb_RestaurantManagerID, tb_RestaurantName, tb_RestaurantAddress };
                resetControls(controls);
            }
        }

        private void bt_RestaurantDishAdd_Click(object sender, EventArgs e)
        {
            List<string> columns = new List<string> { "ID_RESTAURANT_DISHES", "ID_DISH", "ID_RESTAURANT", "PRICE", "WEIGHT" };
            List<string> values = new List<string> { "RESTAURANT_DISHES_ID_RESTAURAN.NEXTVAL", $"'{cb_RestaurantDishesDishID.Text}'", $"'{cb_RestaurantDishesRestaurantID.Text}'", $"'{num_RestaurantDishPrice.Value}'", $"'{num_RestaurantDishWeight.Value}'" };

            if (OracleDB.runInsertCommand("RESTAURANT_DISHES", columns, values))
            {
                initializeComboBoxes();
                List<Control> controls = new List<Control> { cb_RestaurantDishesDishID, cb_RestaurantDishesRestaurantID, num_RestaurantDishPrice, num_RestaurantDishWeight };
                resetControls(controls);
            }
        }

        private void bt_UserDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch(cb_BasedUser.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereUserID;
                    break;
                case 1:
                    cb = cb_WhereUserName;
                    break;
                case 2:
                    cb = cb_WhereUserPhone;
                    break;
            }
            OracleDB.runDeleteCommand("USERS", cb_BasedUser.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_ReviewDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedReview.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereReviewID;
                    break;
            }
            OracleDB.runDeleteCommand("REVIEWS", cb_BasedReview.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_FoodCategoryDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedFoodCategories.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereFoodCategoryID;
                    break;
                case 1:
                    cb = cb_WhereFoodCategoryName;
                    break;
            }
            OracleDB.runDeleteCommand("FOOD_CATEGORIES", cb_BasedFoodCategories.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_DishDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedDish.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereDishID;
                    break;
                case 1:
                    cb = cb_WhereDishName;
                    break;
            }
            OracleDB.runDeleteCommand("DISHES", cb_BasedDish.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_ManagerDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedManager.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereManagerID;
                    break;
                case 1:
                    cb = cb_WhereManagerName;
                    break;
                case 2:
                    cb = cb_WhereManagerPhone;
                    break;
                case 3:
                    cb = cb_WhereManagerMailAddress;
                    break;
            }
            OracleDB.runDeleteCommand("MANAGERS", cb_BasedManager.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_RestaurantDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedRestaurant.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereRestaurantID;
                    break;
                case 1:
                    cb = cb_WhereRestaurantName;
                    break;
                case 2:
                    cb = cb_WhereRestaurantManagerID;
                    break;
            }
            OracleDB.runDeleteCommand("RESTAURANTS", cb_BasedRestaurant.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_RestaurantDishDelete_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedRestaurantDish.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereRestaurantDishID;
                    break;
            }
            OracleDB.runDeleteCommand("RESTAURANT_DISHES", cb_BasedRestaurantDish.Text, cb.Text);
            initializeComboBoxes();
        }

        private void bt_UserEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedUser.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereUserID;
                    break;
                case 1:
                    cb = cb_WhereUserName;
                    break;
                case 2:
                    cb = cb_WhereUserPhone;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if(ch_UserName.Checked)
            {
                colums.Add("NAME");
                values.Add(tb_UserName.Text);
            }
            if(ch_UserPhone.Checked)
            {
                colums.Add("PHONE_NUMBER");
                values.Add(tb_UserPhone.Text);
            }
            OracleDB.runEditCommand("USERS", cb_BasedUser.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_ReviewEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedReview.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereReviewID;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if (ch_ReviewUserID.Checked)
            {
                colums.Add("ID_USER");
                values.Add(cb_ReviewUserID.Text);
            }
            if (ch_ReviewRestaurantDishID.Checked)
            {
                colums.Add("ID_RESTAURANT_DISHES");
                values.Add(cb_ReviewRestaurantDishID.Text);
            }
            if(ch_ReviewScore.Checked)
            {
                colums.Add("SCORE");
                values.Add(num_ReviewScore.Value.ToString());
            }
            if (ch_ReviewDescription.Checked)
            {
                colums.Add("DESCRIPTION");
                values.Add(tb_ReviewDescription.Text);
            }
            OracleDB.runEditCommand("REVIEWS", cb_BasedReview.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_FoodCategoryEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedFoodCategories.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereFoodCategoryID;
                    break;
                case 1:
                    cb = cb_WhereFoodCategoryName;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if (ch_FoodCategoryName.Checked)
            {
                colums.Add("NAME");
                values.Add(tb_FoodCategoryName.Text);
            }
            if (ch_FoodCategoryDescription.Checked)
            {
                colums.Add("DESCRIPTION");
                values.Add(tb_FoodCategoryDescription.Text);
            }
            OracleDB.runEditCommand("FOOD_CATEGORIES", cb_BasedFoodCategories.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_DishEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedDish.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereDishID;
                    break;
                case 1:
                    cb = cb_WhereDishName;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if (ch_DishName.Checked)
            {
                colums.Add("NAME");
                values.Add(tb_DishName.Text);
            }
            if (ch_DishFoodCategoryID.Checked)
            {
                colums.Add("ID_FOOD_CATEGORY");
                values.Add(cb_DishFoodCategoryID.Text);
            }
            if (ch_DishIngredients.Checked)
            {
                colums.Add("INGREDIENTS");
                values.Add(tb_DishIngredients.Text);
            }
            OracleDB.runEditCommand("DISHES", cb_BasedDish.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_ManagerEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedManager.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereManagerID;
                    break;
                case 1:
                    cb = cb_WhereManagerName;
                    break;
                case 2:
                    cb = cb_WhereManagerPhone;
                    break;
                case 3:
                    cb = cb_WhereManagerMailAddress;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if (ch_ManagerName.Checked)
            {
                colums.Add("NAME");
                values.Add(tb_ManagerName.Text);
            }
            if (ch_ManagerPhone.Checked)
            {
                colums.Add("CONTACT_NUMBER");
                values.Add(tb_ManagerPhone.Text);
            }
            if (ch_ManagerMailAddress.Checked)
            {
                colums.Add("MAIL_ADDRESS");
                values.Add(tb_ManagerMailAddress.Text);
            }
            OracleDB.runEditCommand("MANAGERS", cb_BasedManager.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_RestaurantEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedRestaurant.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereRestaurantID;
                    break;
                case 1:
                    cb = cb_WhereRestaurantName;
                    break;
                case 2:
                    cb = cb_WhereRestaurantManagerID;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if (ch_RestaurantName.Checked)
            {
                colums.Add("NAME");
                values.Add(tb_RestaurantName.Text);
            }
            if (ch_RestaurantManagerID.Checked)
            {
                colums.Add("ID_MANAGER");
                values.Add(cb_RestaurantManagerID.Text);
            }
            if (ch_RestaurantAddress.Checked)
            {
                colums.Add("ADDRESS");
                values.Add(tb_RestaurantAddress.Text);
            }
            OracleDB.runEditCommand("RESTAURANTS", cb_BasedRestaurant.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_RestaurantDishEdit_Click(object sender, EventArgs e)
        {
            ComboBox cb = new ComboBox(); cb.Text = "";
            switch (cb_BasedRestaurantDish.SelectedIndex)
            {
                case 0:
                    cb = cb_WhereRestaurantDishID;
                    break;
            }

            List<string> colums = new List<string>(), values = new List<string>();
            if (ch_RestaurantDishesDishID.Checked)
            {
                colums.Add("ID_DISH");
                values.Add(cb_RestaurantDishesDishID.Text);
            }
            if (ch_RestaurantDishesRestaurantID.Checked)
            {
                colums.Add("ID_RESTAURANT");
                values.Add(cb_RestaurantDishesRestaurantID.Text);
            }
            if (ch_RestaurantDishPrice.Checked)
            {
                colums.Add("PRICE");
                values.Add(num_RestaurantDishPrice.Text);
            }
            if (ch_RestaurantDishWeight.Checked)
            {
                colums.Add("WEIGHT");
                values.Add(num_RestaurantDishWeight.Text);
            }
            OracleDB.runEditCommand("RESTAURANT_DISHES", cb_BasedRestaurantDish.Text, cb.Text, colums, values);
            initializeComboBoxes();
        }

        private void bt_Load_Click(object sender, EventArgs e)
        {
            initializeComboBoxes();
        }
    }
}
