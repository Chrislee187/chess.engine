using chess.engine.Movement.King;
using chess.engine.tests.Builders;
using NUnit.Framework;
using Shouldly;

namespace chess.engine.tests.Movement.King
{
    [TestFixture]
    public class KingCastleValidatorTests
    {
        private KingCastleValidator _validator;
        private readonly ChessValidationStepsMocker _stepMocker = new ChessValidationStepsMocker();

        [SetUp]
        public void Setup()
        {
            _validator = new KingCastleValidator(_stepMocker.Build());

            _stepMocker.SetupKingCastleEligibility(false)
                .SetupCastleRookEligibility(false)
                .SetupPathIsClear(false)
                .SetupPathIsSafe(false);
        }
        [Test]
        public void ValidateMove_fails_when_king_is_not_allowed_to_castle()
        {
            _validator.ValidateMove(null, null).ShouldBeFalse();
        }
        [Test]
        public void ValidateMove_fails_when_rook_is_not_allowed_to_castle()
        {
            _stepMocker.SetupKingCastleEligibility(true);

            _validator.ValidateMove(null, null).ShouldBeFalse();
        }
        [Test]
        public void ValidateMove_fails_when_path_between_is_not_clear()
        {
            _stepMocker.SetupKingCastleEligibility(true)
                .SetupCastleRookEligibility(true);

            _validator.ValidateMove(null, null).ShouldBeFalse();
        }
        [Test]
        public void ValidateMove_fails_when_path_between_is_not_safe()
        {
            _stepMocker.SetupKingCastleEligibility(true)
                .SetupCastleRookEligibility(true)
                .SetupPathIsClear(true);

            _validator.ValidateMove(null, null).ShouldBeFalse();
        }
        [Test]
        public void ValidateMove_succeeeds_when_all_steps_pass()
        {
            _stepMocker.SetupKingCastleEligibility(true)
                .SetupCastleRookEligibility(true)
                .SetupPathIsClear(true)
                .SetupPathIsSafe(true);

            _validator.ValidateMove(null, null).ShouldBeTrue();
        }
    }
}