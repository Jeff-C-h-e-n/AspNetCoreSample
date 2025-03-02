using CoinDesk.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.Controllers
{
    [ApiController]
    [Route("api/security")]
    public class SecurityController : ControllerBase
    {
        [HttpPost("encrypt")]
        public IActionResult EncryptData([FromBody] Data data)
        {
            if (string.IsNullOrEmpty(data.Text))
            {
                return BadRequest("Input cannot be empty");
            }

            string encryptedText = AesEncryptionHelper.Encrypt(data.Text);
            return Ok(new { EncryptedData = encryptedText });
        }

        [HttpPost("decrypt")]
        public IActionResult DecryptData([FromBody] Data data)
        {
            if (string.IsNullOrEmpty(data.Text))
            {
                return BadRequest("Input cannot be empty");
            }

            string decryptedText = AesEncryptionHelper.Decrypt(data.Text);
            return Ok(new { DecryptedData = decryptedText });
        }

        public class Data
        {
            public string Text { get; set; }
        }
    }
}