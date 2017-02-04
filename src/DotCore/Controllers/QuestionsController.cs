using System;
using System.Threading.Tasks;
using DotCore.Models;
using DotCore.Portable.BusinessLogic.ApplicationServices;
using DotCore.Portable.BusinessLogic.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotCore.Controllers
{
    [Route("questions")]
    public class QuestionsController : Controller
    {
        #region Fields

        private readonly QuestionsService _questionsService;
        private readonly AnswerService _answerService;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException">
        ///     <paramref name="questionsService" /> or <paramref name="answerService" /> is
        ///     <see langword="null" />
        /// </exception>
        public QuestionsController([NotNull] QuestionsService questionsService, [NotNull] AnswerService answerService)
        {
            if (questionsService == null)
                throw new ArgumentNullException(nameof(questionsService));
            if (answerService == null)
                throw new ArgumentNullException(nameof(answerService));

            _questionsService = questionsService;
            _answerService = answerService;
        }

        #endregion

        #region Methods

        [Route("")]
        public async Task<IActionResult> Index(QuestionStatus status = QuestionStatus.Active)
        {
            return View(await _questionsService.GetQuestionsAsync(status).ConfigureAwait(false));
        }

        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Question(int id)
        {
            var questionDetails = await _questionsService.GetQuestionDetailsAsync(id).ConfigureAwait(false);

            if (questionDetails == null)
                return RedirectToAction("Error", "Home");

            return View("Question", questionDetails);
        }

        [Authorize]
        [Route("{id}")]
        [HttpPost]
        public async Task<IActionResult> Answer(int id, NewAnswer newAnswer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _answerService.AnswerQuestionAsync(id, newAnswer).ConfigureAwait(false);

            return PartialView(result);
        }

        [Authorize]
        [Route("{id}/accept")]
        [HttpPost]
        public async Task<IActionResult> AcceptAnswer(int id, AcceptAnswerModel acceptAnswerModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _answerService.AcceptAnswerAsync(acceptAnswerModel.AnswerId).ConfigureAwait(false);

            return RedirectToAction("Question", new {id});
        }

        #endregion
    }
}