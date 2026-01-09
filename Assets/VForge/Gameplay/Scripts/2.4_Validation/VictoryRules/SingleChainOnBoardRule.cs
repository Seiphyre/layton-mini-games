using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public class SingleChainOnBoardRule : IVictoryRule
    {
        private readonly int _startPieceId;
        private readonly IMatchRule _matchRule;



        public SingleChainOnBoardRule(int startPieceId, IMatchRule matchRule)
        {
            _startPieceId = startPieceId;
            _matchRule = matchRule;
        }

        public VictoryValidationResult Evaluate(GameState gamestate)
        {
            var start = gamestate.BoardState.PlacedPieces.FirstOrDefault(p => p.Id == _startPieceId);
            if (start == null)
                return VictoryValidationResult.Fail("No start piece found");

            var visited = new List<Piece>();
            var current = start;

            while (true)
            {
                visited.Add(current);

                var validNeighbors = GetValidNeighbors(
                    current,
                    gamestate.BoardState,
                    visited);

                if (validNeighbors.Count == 0)
                    break; // chain end

                if (validNeighbors.Count > 1)
                    return VictoryValidationResult.Fail(
                        "Branch detected",
                        validNeighbors);

                current = validNeighbors[0];
            }

            if (visited.Count != gamestate.BoardState.PlacedPieces.Count)
            {
                var unvisited = gamestate.BoardState.PlacedPieces
                    .Where(p => !visited.Contains(p))
                    .ToList();

                return VictoryValidationResult.Fail(
                    "Chain does not cover all pieces",
                    unvisited);
            }

            return VictoryValidationResult.Success();
        }

        private List<Piece> GetValidNeighbors(Piece piece, BoardState boardState, IEnumerable<Piece> visited)
        {
            var neighbors = boardState.GetNeighborPieces(piece);
            var validNeighbors = new List<Piece>();

            foreach (var neighbor in neighbors)
            {
                if (visited.Contains(neighbor))
                    continue;

                if (_matchRule.IsMatch(piece, neighbor))
                    validNeighbors.Add(neighbor);
            }

            return validNeighbors;
        }
    }

}
