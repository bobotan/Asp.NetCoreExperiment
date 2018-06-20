﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestfulStandard01.Model;

namespace RestfulStandard01.Controllers
{

    /// <summary>
    /// 用户Controller
    /// </summary>  
    [Route("api/users/{userId}/accounts/")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        readonly ILogger<AccountsController> _logger;
        /// <summary>
        /// 
        /// </summary>
        readonly IAccountRepository _accountRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="accountRepository"></param>
        public AccountsController(ILogger<AccountsController> logger, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }


        /// <summary>
        /// 按用户获取帐号
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<Account>), 200)]
        [HttpGet]
        public ActionResult GetAccounts(int userId)
        {
            var accounts = _accountRepository.GetAccountsByUserID(userId);
            if (accounts == null || accounts.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(accounts);
            }
        }
        /// <summary>
        /// 按用户ID和帐户ID查询帐户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="accountId">帐户ID</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Account), 200)]
        [HttpGet("{accountId}")]
        public IActionResult GetAccount(int userId, int accountId)
        {
            var account = _accountRepository.GetAccountByID(userId, accountId);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(account);
            }
        }
        /// <summary>
        /// 添加帐户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="account">帐户</param>
        /// <returns></returns>         
        [ProducesResponseType(typeof(Account), 200)]
        [HttpPost]
        public IActionResult AddAccount(int userId, [FromBody]Account account)
        {
            account.UserID = userId;
            var backAccount = _accountRepository.AddAccount(account);
            if (backAccount == null)
            {
                return NotFound();
            }
            else
            {
                return CreatedAtAction("GetAccount", new { userId=backAccount.UserID, accountId = backAccount.ID }, backAccount);
            }
        }
    }

}