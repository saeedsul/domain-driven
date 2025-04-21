import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./Pages/Home";
import ActivitiesPage from "./Pages/ActivitiesPage";
import Orders from "./Pages/Orders";
import Customers from "./Pages/Customers";

const App = () => {
    return (
        <Router>
            <Navbar />
            <div className="container mt-4">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/activities" element={<ActivitiesPage />} />
                    <Route path="/orders" element={<Orders />} />
                    <Route path="/customers" element={<Customers />} />
                </Routes>
            </div>
        </Router>
    );
};

export default App;
