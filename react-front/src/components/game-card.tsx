import { Link } from "react-router-dom";
import type { Game } from "../types/game";

interface GameCardProps {
  game: Game;
}

export const GameCard = function ({ game }: GameCardProps) {
  const averageRating = game.averageRating?.toFixed(1) ?? "-";

  return (
    <article className="game-card">
      <Link to={`/gameCatalog/${game.id}`} className="game-card__media">
        {game.imageUrl ? (
          <img src={game.imageUrl} alt={game.title} />
        ) : (
          <div className="game-card__no-image">No image</div>
        )}
      </Link>
      <div className="game-card__body">
        <h3 className="game-card__title">
          <Link to={`/gameCatalog/${game.id}`}>{game.title}</Link>
        </h3>
        {game.genres.length > 0 && (
          <div className="game-card__genres">
            {game.genres.map((genre) => (
              <span key={genre} className="game-card__genre">
                {genre}
              </span>
            ))}
          </div>
        )}
        <div className="game-card__stats">
          <span className="game-card__price">${game.price.toFixed(2)}</span>
          <span>{averageRating}/10</span>
          <span>{game.reviewsCount} reviews</span>
        </div>
      </div>
    </article>
  );
};
