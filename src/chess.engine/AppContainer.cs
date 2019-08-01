using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.King;
using chess.engine.SAN;
using Microsoft.Extensions.DependencyInjection;

namespace chess.engine
{
    public static class AppContainer
    {
        public static readonly ServiceCollection ServiceCollection;
        public static ServiceProvider ServiceProvider { get; }

        static AppContainer()
        {
            ServiceCollection = new ServiceCollection();
            AddChessDependencies(ServiceCollection);

            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public static T GetService<T>()
            => ServiceProvider.GetService<T>();

        public static void AddChessDependencies(this IServiceCollection services)
        {
            // board.engine generic dependencies
            services.AddTransient<IRefreshAllPaths<ChessPieceEntity>, ChessRefreshAllPaths>();
            services.AddTransient<IPathsValidator<ChessPieceEntity>, ChessPathsValidator>();
            services.AddTransient<IPathValidator<ChessPieceEntity>,ChessPathValidator>();
            services.AddTransient<IMoveValidationProvider<ChessPieceEntity>, ChessMoveValidationProvider>();
            services.AddTransient<IBoardActionProvider<ChessPieceEntity>,ChessBoardActionProvider>();
            services.AddTransient<IBoardEntityFactory<ChessPieceEntity>,ChessPieceEntityFactory>();
            services.AddTransient<IBoardMoveService<ChessPieceEntity>,BoardMoveService<ChessPieceEntity>>();
            services.AddTransient<IBoardEngineProvider<ChessPieceEntity>,ChessBoardEngineProvider>();

            // chess.engine specific dependencies
            services.AddTransient<IPlayerStateService, PlayerStateService>();
            services.AddTransient<ICheckDetectionService, CheckDetectionService>();
            services.AddTransient<ISanTokenParser, SanTokenParser>();
            services.AddTransient<IChessValidationSteps, ChessValidationSteps>();
            services.AddTransient<ISanBuilder, SanBuilder>();

            // NOTE: Acts as a cache, hence singleton usage
            services.AddSingleton<IFindAttackPaths, FindAttackPaths>();

        }
    }
}