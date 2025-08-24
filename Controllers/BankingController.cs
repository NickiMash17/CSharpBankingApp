using Microsoft.AspNetCore.Mvc;
using CSharpBankingApp.Services;
using CSharpBankingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CSharpBankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController : ControllerBase
    {
        private readonly BankService _bankService;

        public BankingController(BankService bankService)
        {
            _bankService = bankService;
        }

        // GET: api/banking/accounts
        [HttpGet("accounts")]
        public ActionResult<List<Account>> GetAllAccounts()
        {
            try
            {
                var accounts = _bankService.GetAllAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve accounts", details = ex.Message });
            }
        }

        // GET: api/banking/accounts/{id}
        [HttpGet("accounts/{id}")]
        public ActionResult<Account> GetAccount(string id)
        {
            try
            {
                var account = _bankService.GetAccount(id);
                if (account == null)
                    return NotFound(new { error = "Account not found" });

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve account", details = ex.Message });
            }
        }

        // POST: api/banking/accounts
        [HttpPost("accounts")]
        public ActionResult<Account> CreateAccount([FromBody] CreateAccountRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Pin))
                    return BadRequest(new { error = "Name and PIN are required" });

                var account = _bankService.CreateAccount(request.Name, request.Pin, request.Type);
                if (account == null)
                    return BadRequest(new { error = "Failed to create account" });

                return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create account", details = ex.Message });
            }
        }

        // POST: api/banking/login
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            try
            {
                var account = _bankService.FindAccountByName(request.Name);
                if (account == null)
                    return Unauthorized(new { error = "Account not found" });

                if (!_bankService.VerifyPin(account.Id, request.Pin))
                    return Unauthorized(new { error = "Invalid PIN" });

                return Ok(new LoginResponse
                {
                    Account = account,
                    Message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Login failed", details = ex.Message });
            }
        }

        // POST: api/banking/accounts/{id}/deposit
        [HttpPost("accounts/{id}/deposit")]
        public ActionResult<TransactionResponse> Deposit(string id, [FromBody] TransactionRequest request)
        {
            try
            {
                if (!_bankService.VerifyPin(id, request.Pin))
                    return Unauthorized(new { error = "Invalid PIN" });

                if (_bankService.Deposit(id, request.Amount, request.Pin))
                {
                    var account = _bankService.GetAccount(id);
                    return Ok(new TransactionResponse
                    {
                        Success = true,
                        Message = $"Successfully deposited {request.Amount:C}",
                        NewBalance = account?.Balance ?? 0
                    });
                }

                return BadRequest(new { error = "Deposit failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Deposit failed", details = ex.Message });
            }
        }

        // POST: api/banking/accounts/{id}/withdraw
        [HttpPost("accounts/{id}/withdraw")]
        public ActionResult<TransactionResponse> Withdraw(string id, [FromBody] TransactionRequest request)
        {
            try
            {
                if (!_bankService.VerifyPin(id, request.Pin))
                    return Unauthorized(new { error = "Invalid PIN" });

                if (_bankService.Withdraw(id, request.Amount, request.Pin))
                {
                    var account = _bankService.GetAccount(id);
                    return Ok(new TransactionResponse
                    {
                        Success = true,
                        Message = $"Successfully withdrew {request.Amount:C}",
                        NewBalance = account?.Balance ?? 0
                    });
                }

                return BadRequest(new { error = "Withdrawal failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Withdrawal failed", details = ex.Message });
            }
        }

        // POST: api/banking/transfer
        [HttpPost("transfer")]
        public ActionResult<TransactionResponse> Transfer([FromBody] TransferRequest request)
        {
            try
            {
                if (!_bankService.VerifyPin(request.FromAccountId, request.Pin))
                    return Unauthorized(new { error = "Invalid PIN" });

                if (_bankService.Transfer(request.FromAccountId, request.ToAccountId, request.Amount, request.Pin))
                {
                    var account = _bankService.GetAccount(request.FromAccountId);
                    return Ok(new TransactionResponse
                    {
                        Success = true,
                        Message = $"Successfully transferred {request.Amount:C}",
                        NewBalance = account?.Balance ?? 0
                    });
                }

                return BadRequest(new { error = "Transfer failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Transfer failed", details = ex.Message });
            }
        }

        // POST: api/banking/accounts/{id}/change-pin
        [HttpPost("accounts/{id}/change-pin")]
        public ActionResult<BaseResponse> ChangePin(string id, [FromBody] ChangePinRequest request)
        {
            try
            {
                if (_bankService.ChangePin(id, request.CurrentPin, request.NewPin))
                {
                    return Ok(new BaseResponse
                    {
                        Success = true,
                        Message = "PIN changed successfully"
                    });
                }

                return BadRequest(new { error = "Failed to change PIN" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to change PIN", details = ex.Message });
            }
        }

        // POST: api/banking/accounts/{id}/convert-type
        [HttpPost("accounts/{id}/convert-type")]
        public ActionResult<BaseResponse> ConvertAccountType(string id, [FromBody] ConvertTypeRequest request)
        {
            try
            {
                if (!_bankService.VerifyPin(id, request.Pin))
                    return Unauthorized(new { error = "Invalid PIN" });

                if (_bankService.ConvertAccountType(id, request.NewType, request.Pin))
                {
                    return Ok(new BaseResponse
                    {
                        Success = true,
                        Message = $"Account successfully converted to {request.NewType}"
                    });
                }

                return BadRequest(new { error = "Failed to convert account type" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to convert account type", details = ex.Message });
            }
        }

        // GET: api/banking/backups
        [HttpGet("backups")]
        public ActionResult<List<string>> GetBackups()
        {
            try
            {
                var backups = _bankService.GetAvailableBackups();
                return Ok(backups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve backups", details = ex.Message });
            }
        }

        // POST: api/banking/backups
        [HttpPost("backups")]
        public ActionResult<BaseResponse> CreateBackup()
        {
            try
            {
                _bankService.CreateBackup();
                return Ok(new BaseResponse
                {
                    Success = true,
                    Message = "Backup created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create backup", details = ex.Message });
            }
        }

        // POST: api/banking/backups/restore
        [HttpPost("backups/restore")]
        public ActionResult<BaseResponse> RestoreBackup([FromBody] RestoreBackupRequest request)
        {
            try
            {
                if (_bankService.RestoreFromBackup(request.BackupFileName))
                {
                    return Ok(new BaseResponse
                    {
                        Success = true,
                        Message = "Backup restored successfully"
                    });
                }

                return BadRequest(new { error = "Failed to restore backup" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to restore backup", details = ex.Message });
            }
        }
    }

    // Request/Response Models
    public class CreateAccountRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(4, MinimumLength = 4)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be exactly 4 digits")]
        public string Pin { get; set; } = string.Empty;
        
        public AccountType Type { get; set; } = AccountType.Savings;
    }

    public class LoginRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Pin { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public Account Account { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
    }

    public class TransactionRequest
    {
        public decimal Amount { get; set; }
        public string Pin { get; set; } = string.Empty;
    }

    public class TransactionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal NewBalance { get; set; }
    }

    public class TransferRequest
    {
        public string FromAccountId { get; set; } = string.Empty;
        public string ToAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Pin { get; set; } = string.Empty;
    }

    public class ChangePinRequest
    {
        public string CurrentPin { get; set; } = string.Empty;
        public string NewPin { get; set; } = string.Empty;
    }

    public class ConvertTypeRequest
    {
        public AccountType NewType { get; set; }
        public string Pin { get; set; } = string.Empty;
    }

    public class RestoreBackupRequest
    {
        public string BackupFileName { get; set; } = string.Empty;
    }

    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
} 