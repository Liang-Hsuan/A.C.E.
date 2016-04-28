using System;
using System.Data.SqlClient;

namespace AshlinCustomerEnquiry.supportingClasses
{
    /*
     * A class that calculate the price 
     */
    public static class Price
    {
        // fields for storing discount matrix values
        private static readonly double[][] List = new double[8][];

        /* constructor that initialize discount matrix list field */
        static Price()
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs);

            // [0] 1 net standard, [1] 6 net standard, [2] 24 net standard, [3] 50 net standard, [4] 100 net standard, [5] 250 net standard, [6] 500 net standard, [7] 1000 net standard, [8] 2500 net standard, [9] rush net
            SqlCommand command = new SqlCommand("SELECT [1_Net_Standard Delivery], [6_Net_Standard Delivery], [24_Net_Standard Delivery], [50_Net_Standard Delivery], [100_Net_Standard Delivery], [250_Net_Standard Delivery], [500_Net_Standard Delivery], [1000_Net_Standard Delivery], [2500_Net_Standard Delivery], [RUSH_Net_25_wks] "
                                              + "FROM Discount_Matrix", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; i <= 6; i++)
            {
                double[] itemList = new double[10];
                reader.Read();
                for (int j = 0; j <= 9; j++)
                {
                    try
                    {
                        itemList[j] = reader.GetDouble(j);
                    }
                    catch
                    {
                        itemList[j] = 0;
                    }
                }
                List[i] = itemList;
            }
            reader.Close();

            // [5][0] multiplier
            command.CommandText = "SELECT [MSRP Multiplier] FROM ref_msrp_multiplier";
            reader = command.ExecuteReader();
            reader.Read();
            List[7] = new[] { reader.GetDouble(0), 0 };
            reader.Close();

            // [5][1] USD currency
            command.CommandText = "SELECT Value FROM Currency WHERE Currency = 'USD'";
            reader = command.ExecuteReader();
            reader.Read();
            List[7][1] = reader.GetDouble(0);
            connection.Close();
        }

        /* a method that return the price from the given information of the product */
        public static double GetPrice(double basePrice, int pricingTier, int quantity, bool imprint, bool rush, bool oversea)
        {
            // first get the base price of the sku and calculate msrp -> msrp will also be the return value
            double msrp = List[7][0] * basePrice;

            // get the corresponding row number for the pricing tier
            int row;
            switch (pricingTier)
            {
                case 1:
                    row = 1;
                    break;
                case 2:
                    row = 2;
                    break;
                case 3:
                    row = 3;
                    break;
                case 4:
                    row = 4;
                    break;
                case 5:
                    row = 5;
                    break;
                case 6:
                    row = 6;
                    break;
                default:
                    row = 0;
                    break;
            }

            if (imprint)
            {
                // the case if it is imprinted
                // calculate run charge
                double runcharge = (msrp * 0.05) / 0.6;
                if (runcharge > 8)
                    runcharge = 8;
                else if (runcharge < 1)
                    runcharge = 1;

                if (rush)
                {
                    // the case if it is rush
                    if (quantity < 6)
                        msrp = (msrp + runcharge) * List[row][0] * List[row][9];
                    if (quantity >= 6 && quantity < 24)
                        msrp = (msrp + runcharge) * List[row][1] * List[row][9];
                    if (quantity >= 24 && quantity < 50)
                        msrp = (msrp + runcharge) * List[row][2] * List[row][9];
                    if (quantity >= 50 && quantity < 100)
                        msrp = (msrp + runcharge) * List[row][3] * List[row][9];
                    if (quantity >= 100 && quantity < 250)
                        msrp = (msrp + runcharge) * List[row][4] * List[row][9];
                    if (quantity >= 250 && quantity < 500)
                        msrp = (msrp + runcharge) * List[row][5] * List[row][9];
                    if (quantity >= 500 && quantity < 1000)
                        msrp = (msrp + runcharge) * List[row][6] * List[row][9];
                    if (quantity >= 1000 && quantity < 2500)
                        msrp = (msrp + runcharge) * List[row][7] * List[row][9];
                    if (quantity >= 2500)
                        msrp = (msrp + runcharge) * List[row][8] * List[row][9];
                }
                else
                {
                    // the case if it is not rush
                    if (quantity < 6)
                        msrp = (msrp + runcharge) * List[row][0];
                    if (quantity >= 6 && quantity < 24)
                        msrp = (msrp + runcharge) * List[row][1];
                    if (quantity >= 24 && quantity < 50)
                        msrp = (msrp + runcharge) * List[row][2];
                    if (quantity >= 50 && quantity < 100)
                        msrp = (msrp + runcharge) * List[row][3];
                    if (quantity >= 100 && quantity < 250)
                        msrp = (msrp + runcharge) * List[row][4];
                    if (quantity >= 250 && quantity < 500)
                        msrp = (msrp + runcharge) * List[row][5];
                    if (quantity >= 500 && quantity < 1000)
                        msrp = (msrp + runcharge) * List[row][6];
                    if (quantity >= 1000 && quantity < 2500)
                        msrp = (msrp + runcharge) * List[row][7];
                    if (quantity >= 2500)
                        msrp = (msrp + runcharge) * List[row][8];
                }
            }
            else
            {
                // the case if it is blank
                if (rush)
                {
                    // the case if it is rush
                    if (quantity < 6)
                        msrp *= List[row][0] * List[row][9];
                    else if (quantity >= 6 && quantity < 24)
                        msrp *= List[row][1] * List[row][9];
                    else if (quantity >= 24 && quantity < 50)
                        msrp *= List[row][2] * List[row][9];
                    else if (quantity >= 50 && quantity < 100)
                        msrp *= List[row][3] * List[row][9];
                    else if (quantity >= 100 && quantity < 250)
                        msrp *= List[row][4] * List[row][9];
                    else if (quantity >= 250 && quantity < 500)
                        msrp *= List[row][5] * List[row][9];
                    else if (quantity >= 500 && quantity < 1000)
                        msrp *= List[row][6] * List[row][9];
                    else if (quantity >= 1000 && quantity < 2500)
                        msrp *= List[row][7] * List[row][9];
                    else
                        msrp *= List[row][8] * List[row][9];
                }
                else
                {
                    // the case if it is not rush
                    if (quantity < 6)
                        msrp *= List[row][0];
                    else if (quantity >= 6 && quantity < 24)
                        msrp *= List[row][1];
                    else if (quantity >= 24 && quantity < 50)
                        msrp *= List[row][2];
                    else if (quantity >= 50 && quantity < 100)
                        msrp *= List[row][3];
                    else if (quantity >= 100 && quantity < 250)
                        msrp *= List[row][4];
                    else if (quantity >= 250 && quantity < 500)
                        msrp *= List[row][5];
                    else if (quantity >= 500 && quantity < 1000)
                        msrp *= List[row][6];
                    else if (quantity >= 1000 && quantity < 2500)
                        msrp *= List[row][7];
                    else
                        msrp *= List[row][8];
                }
            }

            return oversea ? msrp * List[7][1] : msrp;
        }

        /* a supporting method that return the base price of the given sku */
        public static double GetPrice(string sku)
        {
            double basePrice;

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Designcs))
            {
                SqlCommand command = new SqlCommand("SELECT Base_Price FROM master_SKU_Attributes WHERE SKU_Ashlin = \'" + sku + '\'', connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                basePrice = Convert.ToDouble(reader.GetValue(0));
            }

            return basePrice;
        }
    }
}