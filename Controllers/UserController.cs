using server.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Interface.Repository;
using server.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN,USER")]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IUserRepository userRepository;
        public UserController(IUserRepository userRepository,IMapper mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }
        [HttpGet]
        [Route("GetAllAddressByUserId/{userId}")]
        public async Task<ActionResult<ResponseDto>> GetAllAddressByUserId(int userId)
        {

            IEnumerable<AddressDto> addresses = mapper.Map<IEnumerable<AddressDto>>(
                await this.userRepository.GetAllAddressByUserId(userId)
            );

            ResponseDto responseDto = new ResponseDto();
            return Ok(
                responseDto.success(
                    "Success",
                    addresses
                )
            );
        }
        [HttpPost("AddAddress")]
        public async Task<ActionResult<ResponseDto>> AddAddress([FromBody] AddAddressDto address){
            ResponseDto responseDto = new ResponseDto();
            Address address1 = mapper.Map<Address>(address);
             await this.userRepository.AddAddress(address1);
            return Ok(responseDto.success("Success"));
        }
        [HttpDelete("DeleteAddress/{addressId}")]
        public async Task<ActionResult<ResponseDto>> DeleteAddress(int addressId){
            ResponseDto responseDto = new ResponseDto();
             await this.userRepository.RemoveAddress(addressId);
            return Ok(responseDto.success("Success"));
        }
    }
}
