using System;
using System.Threading.Tasks;
using DotCore.Portable.BusinessLogic.ApplicationServices;
using DotCore.Portable.BusinessLogic.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotCore.Controllers
{
    [Authorize]
    public class AskController : Controller
    {
        #region Fields

        private readonly AskService _askService;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException"><paramref name="askService"/> is <see langword="null"/></exception>
        public AskController([NotNull] AskService askService)
        {
            if (askService == null)
                throw new ArgumentNullException(nameof(askService));

            _askService = askService;
        }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return View(new AskQuestionModel());
        }

        public async Task<IActionResult> Ask(AskQuestionModel askQuestionModel)
        {
            if (!ModelState.IsValid)
                return View("Index", askQuestionModel);

            var question = await _askService.AskQuestionAsync(askQuestionModel).ConfigureAwait(false);

            return RedirectToAction("Question", "Questions", new {id = question.Id});
        }

        #endregion
    }
}