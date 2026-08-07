import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getGame } from "../services/game-service";
import type { Game } from "../types/game";

export const GameDetail = () => {
  const { id } = useParams();
  const [game, setGame] = useState<Game | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    const gameId = Number(id);

    if (!gameId) {
      setError("Wrong game id");
      setLoading(false);
      return;
    }

    const load = async () => {
      setLoading(true);

      try {
        const data = await getGame(gameId);

        if (!cancelled) {
          setGame(data);
          setError(data ? null : "Game not found");
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof Error ? err.message : "Can't load game");
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    };

    load();

    return () => {
      cancelled = true;
    };
  }, [id]);

  if (loading) {
    return <p className="game-detail__status">Loading...</p>;
  }

  if (error || !game) {
    return (
      <div className="game-detail">
        <p className="game-detail__status game-detail__status--error">
          {error ?? "Game not found"}
        </p>
        <Link to="/gameCatalog" className="game-detail__back">
          ← Back to catalog
        </Link>
      </div>
    );
  }

  return (
    <article className="game-detail">
      <Link to="/gameCatalog" className="game-detail__back">
        ← Back to catalog
      </Link>

      <div className="game-detail__layout">
        <div className="game-detail__media">
          {game.imageUrl ? (
            <img src={game.imageUrl} alt={game.title} />
          ) : (
            <div className="game-detail__no-image">No image</div>
          )}
        </div>

        <div className="game-detail__info">
          <h1 className="game-detail__title">{game.title}</h1>

          {game.genres.length > 0 && (
            <div className="game-detail__genres">
              {game.genres.map((genre) => (
                <span key={genre} className="game-detail__genre">
                  {genre}
                </span>
              ))}
            </div>
          )}

          <p className="game-detail__price">${game.price.toFixed(2)}</p>

          {game.description && (
            <p className="game-detail__description">{game.description}</p>
          )}
        </div>
      </div>
    </article>
  );
};
