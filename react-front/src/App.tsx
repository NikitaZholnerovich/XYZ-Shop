import "./App.css";
import { Link, Route, Routes } from "react-router-dom";
import { GameCatalog } from "./pages/GameCatalog";
import { Home } from "./pages/Home";
import { GameDetail } from "./pages/GameDetail";

function App() {
  return (
    <div className="app">
      <header className="app-header">
        <span className="app-header__logo">XYZ Shop</span>
        <nav className="app-nav">
          <Link to="/">Home</Link>
          <Link to="/gameCatalog">Catalog</Link>
        </nav>
      </header>

      <main className="app-main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/gameCatalog" element={<GameCatalog />} />
          <Route path="/gameCatalog/:id" element={<GameDetail />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
