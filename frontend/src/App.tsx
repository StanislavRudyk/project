import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import { AnimatePresence, motion } from 'framer-motion';

import Home from './pages/Home';
import Signup from './pages/Signup';
import Clubs from './pages/Clubs';
import Leaderboard from './pages/Leaderboard';
import Profile from './pages/Profile';
import MoodTracker from './pages/MoodTracker';
import BookDetail from './pages/BookDetail';
import BookDebates from './pages/BookDebates';

const pageVariants = {
  initial: { opacity: 0, y: 16 },
  animate: { opacity: 1, y: 0, transition: { duration: 0.28, ease: [0.22, 1, 0.36, 1] } },
  exit:    { opacity: 0, y: -8, transition: { duration: 0.18, ease: 'easeIn' } },
};

function AnimatedRoutes() {
  const location = useLocation();
  return (
    <AnimatePresence mode="wait">
      <motion.div key={location.pathname} variants={pageVariants} initial="initial" animate="animate" exit="exit">
        <Routes location={location}>
          <Route path="/"              element={<Home />} />
          <Route path="/signup"        element={<Signup />} />
          <Route path="/clubs"         element={<Clubs />} />
          <Route path="/leaderboard"   element={<Leaderboard />} />
          <Route path="/profile"       element={<Profile />} />
          <Route path="/mood"          element={<MoodTracker />} />
          <Route path="/book/:id"      element={<BookDetail />} />
          <Route path="/debates"       element={<BookDebates />} />
        </Routes>
      </motion.div>
    </AnimatePresence>
  );
}

function App() {
  return (
    <Router>

      <AnimatedRoutes />
    </Router>
  );
}

export default App;