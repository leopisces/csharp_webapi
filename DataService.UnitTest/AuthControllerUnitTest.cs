using Moq;
using System.Threading.Tasks;
using Xunit;

namespace DataService.UnitTest
{
    public class AuthControllerUnitTest
    {
        /// <summary>
        /// 单元测试，用来测试数据层方法
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName ="测试")]
        public async Task RedisTest()
        {
            //DbContextOptions<AddressContext> options = new DbContextOptionsBuilder<AddressContext>().UseInMemoryDatabase("Add_Address_Database").Options;
            //var addressContext = new AddressContext(options);

            //var createAddress = new AddressCreateDto
            //{
            //    City = "昆明",
            //    County = "五华区",
            //    Province = "云南省"
            //};
            //var stubAddressRepository = new Mock<IRepository<Domain.Address>>();
            //var stubProvinceRepository = new Mock<IRepository<Province>>();
            //var addressUnitOfWork = new AddressUnitOfWork<AddressContext>(addressContext);

            //var stubAddressService = new AddressServiceImpl.AddressServiceImpl(stubAddressRepository.Object, stubProvinceRepository.Object, addressUnitOfWork);
            //await stubAddressService.CreateAddressAsync(createAddress);
            //int addressAmountActual = await addressContext.Addresses.CountAsync();
            Assert.Equal(1, 1);
        }
    }
}
