using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Employee_archive.tests
{
    [TestClass]
    public class DatabaseHelperTests
    {
        private readonly Mock<IDatabaseHelper> mockDb;

        public DatabaseHelperTests()
        {
            mockDb = new Mock<IDatabaseHelper>();
        }


        [TestMethod]
        public void GetAllEmployees_ShouldReturnMockData()
        {
            //arrange
            var expectedEmployees = new List<Employee>
            {
                new Employee { ID_employee = 1, Full_Name = "Иванов И.И.", RoleName = "Кассир" },
                new Employee { ID_employee = 2, Full_Name = "Петров П.П.", RoleName = "Менеджер" }
            };

            mockDb.Setup(x=>x.GetAllEmployees()).Returns(expectedEmployees);


            //act
            var result = mockDb.Object.GetAllEmployees();

            //assert

            Assert.AreEqual(2,result.Count);
            Assert.AreEqual(expectedEmployees[0].ID_employee, result[0].ID_employee);
            Assert.AreEqual(expectedEmployees[0].Full_Name, result[0].Full_Name);
            Assert.AreEqual(expectedEmployees[1].ID_employee, result[1].ID_employee);
            Assert.AreEqual(expectedEmployees[1].Full_Name, result[1].Full_Name);

        }

        [TestMethod]
        public void Authenticate_WithValidData_ShouldReturnAdmin()
        {
            //arrange
            var expectedAdmin = new Administrator
            {
                ID_Admin = 1,
                Full_Name = "Admin",
                Login = "admin"
            };
            mockDb.Setup(x =>x.Authenticate("admin","1234")).Returns(expectedAdmin);


            //act
            var result = mockDb.Object.Authenticate("admin","1234");

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Admin",result.Full_Name);
            Assert.AreEqual(1,result.ID_Admin);

        }
        

    }
}
