namespace Ordering.Infrastructure.Data.Extensions
{
    public static class InitialData
    {
        public static IEnumerable<Customer> Customers =>
            new List<Customer>
            {
                Customer.Create(CustomerId.Of(new Guid("cdbcbd30-3aa3-46cc-aaa5-857fb4033d2c")), "Mr.David", "mrdavid1023@gmail.com"),
                Customer.Create(CustomerId.Of(new Guid("d96aa930-a6f8-4ec9-bb12-cfdc75131f0a")), "Đăng Khoa", "dangkhoa2002@gmail.com")
            };

        public static IEnumerable<Product> Products =>
            new List<Product> {
                Product.Create(ProductId.Of(new Guid("22b38c70-af56-4783-8ccc-4c1327aac82b")), "Laptop", 1500),
                Product.Create(ProductId.Of(new Guid("4f328784-6d45-4f19-bb58-dc2967218692")), "Smartphone", 800),
                Product.Create(ProductId.Of(new Guid("2d8b65ef-b11f-40be-a626-578106a3ef4d")), "Tablet", 600),
                Product.Create(ProductId.Of(new Guid("2ab565ef-b11f-40be-a626-578106a3ef53")), "Headphones", 200),
            };

        public static IEnumerable<Order> OrderWithItems
        {
            get
            {
                var billingAddress1 = Address.Of("Mr", "David", "mrdavid1023@gmail.com", "To Hien Thanh, Ho Chi Minh", "VietNam", "VietName", "700000");
                var billingAddress2 = Address.Of("Dang", "Khoa", "dangkhoa2002@gmail.com", "Dong Khoi, Ha Noi", "VietName", "VietName", "745020");

                var shippingAddress1 = Address.Of("Thu", "Huong", "thuhuong1200@gmail.com", "To Hien Thanh, Ho Chi Minh", "VietNam", "VietName", "700000");
                var shippingAddress2 = Address.Of("Thuy", "Nga", "thuyngaparis@gmail.com", "Dong Khoi, Ha Noi", "VietName", "VietName", "745020");

                var payment1 = Payment.Of("Credit Card", "1234-5678-9012-3456", "12/25", "543", 3);
                var payment2 = Payment.Of("VNPAY QR Code", "1234-2398-1234-1234", "06/25", "221", 3);

                var order1 = Order.Create(
                        OrderName.Of("Order 1"),
                        CustomerId.Of(new Guid("cdbcbd30-3aa3-46cc-aaa5-857fb4033d2c")),
                        billingAddress1,
                        shippingAddress1,
                        payment1
                    );

                order1.AddOrderItem(ProductId.Of(new Guid("22b38c70-af56-4783-8ccc-4c1327aac82b")), 2, 3000);
                order1.AddOrderItem(ProductId.Of(new Guid("4f328784-6d45-4f19-bb58-dc2967218692")), 3, 2400);


                var order2 = Order.Create(
                        OrderName.Of("Order 2"),
                        CustomerId.Of(new Guid("d96aa930-a6f8-4ec9-bb12-cfdc75131f0a")),
                        shippingAddress2,
                        billingAddress2,
                        payment2
                    );

                order2.AddOrderItem(ProductId.Of(new Guid("2d8b65ef-b11f-40be-a626-578106a3ef4d")), 3, 1800);
                order2.AddOrderItem(ProductId.Of(new Guid("2ab565ef-b11f-40be-a626-578106a3ef53")), 3, 1800);

                return new List<Order> { order1, order2 };
            }
        }
    }
}
