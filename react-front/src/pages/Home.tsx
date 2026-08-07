import { Link } from "react-router-dom";

export const Home = () => {
  return (
    <section className="home">
      <h1 className="home__title">XYZ Shop</h1>
      <p className="home__text">React front-end for the game catalog</p>
      <Link to="/gameCatalog" className="home__link">
        Open catalog
      </Link>
    </section>
  );
};
