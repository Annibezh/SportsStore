using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;
            //Action
            IEnumerable<Product> result = (IEnumerable<Product>)controller.List(null, 2).Model;
            //Assets
            Product[] prodArr = result.ToArray();
            //Assert.IsTrue(prodArr.Length == 3);
            //Assert.AreEqual(prodArr[0].Name, "P3");
            //Assert.AreEqual(prodArr[1].Name, "P4");
        }
        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            //Arrange - create a contoller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;
            //Action
            Product[] result = ((ProductsListVM)controller.List("Cat2", 1).Model).Products.ToArray();
            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }.AsQueryable());
            //Arrange - create the controller
            NavController target = new NavController(mock.Object);
            //Act = get the set of categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();
            //Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //Arrange - create the mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] 
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                    new Product {ProductID = 4, Name = "P2", Category = "Oranges"},
                }.AsQueryable());
            // Arrange - create the controller   
            NavController target = new NavController(mock.Object);
            // Arrange - define the category to selected   
            string categoryToSelect = "Apples";
            // Action   
                //string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;
            // Assert   
                //Assert.AreEqual(categoryToSelect, result); 
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange - create the mock repository   
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] 
                {
                    new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                    new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
                }.AsQueryable());
            // Arrange - create a controller and make the page size 3 items   
            ProductController target = new ProductController(mock.Object);
            target.pageSize = 3;
            // Action - test the product counts for different categories   
            int res1 = ((ProductsListVM)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListVM)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListVM)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListVM)target.List(null).Model).PagingInfo.TotalItems;
            // Assert   
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5); 
        }
    }
}
