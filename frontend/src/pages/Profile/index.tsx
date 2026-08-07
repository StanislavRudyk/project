import { useState } from 'react';
import { Header } from '../../components/Header';
import { Link } from 'react-router-dom';
import { motion } from 'framer-motion';
import { UserLevel } from '../../components/UserLevel';
import { AvatarBuilder } from '../../components/AvatarBuilder';
import styles from './Profile.module.css';

/* ─── Knowledge Tree data ─── */
const TREE_NODES = [
  { id: 'fiction', label: 'Fiction', icon: '📖', size: 60, color: '#4F46E5', x: '50%', y: '15%', books: 21 },
  { id: 'scifi', label: 'Sci-Fi', icon: '🌌', size: 50, color: '#0369A1', x: '20%', y: '38%', books: 12 },
  { id: 'nonfic', label: 'Non-Fiction', icon: '🧠', size: 48, color: '#059669', x: '78%', y: '38%', books: 9 },
  { id: 'mystery', label: 'Mystery', icon: '🔍', size: 40, color: '#DC2626', x: '10%', y: '62%', books: 6 },
  { id: 'poetry', label: 'Poetry', icon: '✍️', size: 34, color: '#BE185D', x: '38%', y: '65%', books: 3 },
  { id: 'history', label: 'History', icon: '🏛️', size: 38, color: '#92400E', x: '65%', y: '62%', books: 5 },
  { id: 'design', label: 'Design', icon: '🎨', size: 32, color: '#D97706', x: '88%', y: '65%', books: 4 },
];

/* ─── Bingo card ─── */
const BINGO_CELLS = [
  'A debut novel', 'Author from Africa', 'Under 200 pages', 'Non-fiction', 'A classic (pre-1970)',
  'Recommended by friend', 'Cover you love', 'Set in a city', 'Sci-Fi or Fantasy', 'A memoir',
  'Award winner', 'Translated book', '⭐ FREE ⭐', 'A thriller', 'Poetry collection',
  'Graphic novel', 'Historical fiction', 'Reread a favorite', 'Published this year', 'By a local author',
  'Over 500 pages', 'One-sitting read', 'Audiobook', 'A biography', 'Nature / Environment',
];

const STATS = [
  { value: '47', label: 'Books Read' },
  { value: '39', label: 'Reviews Written' },
  { value: '28', label: 'Day Streak 🔥' },
  { value: '1.2k', label: 'Pages This Month' },
];

const BADGES = [
  { icon: '🏆', name: 'Top Reader', locked: false },
  { icon: '❤️', name: 'First Like', locked: false },
  { icon: '🔥', name: '7-Day Streak', locked: false },
  { icon: '🔖', name: 'Bookmarked', locked: false },
  { icon: '🎲', name: 'Lucky Pick', locked: false },
  { icon: '🌍', name: 'World Reader', locked: true },
  { icon: '✍️', name: 'Critic', locked: true },
  { icon: '🏛️', name: 'Club Master', locked: true },
];

const container = { animate: { transition: { staggerChildren: 0.06 } } };
const fadeUp = {
  initial: { opacity: 0, y: 20 },
  animate: { opacity: 1, y: 0, transition: { duration: 0.4, ease: [0.22, 1, 0.36, 1] } },
};

export default function Profile() {
  const [completed, setCompleted] = useState<Set<number>>(new Set([12]));

  const toggle = (i: number) => {
    if (i === 12) return;
    setCompleted(prev => {
      const next = new Set(prev);
      next.has(i) ? next.delete(i) : next.add(i);
      return next;
    });
  };

  const progress = completed.size;
  const pct = Math.round((progress / 25) * 100);

  return (
    <div className={styles.page}>
      <Header />

      <div className={styles.hero}>
        <div className={styles.kicker}><span></span> My Profile</div>
        <div className={styles.profileRow}>
          <UserLevel initials="JD" booksRead={47} size={72} />
          <div className={styles.profileInfo}>
            <div className={styles.profileName}>Jane Doe</div>
            <div className={styles.profileSub}>
              <span className={styles.statusDot}></span>
              Currently reading · Dune by Frank Herbert
            </div>
            <Link to="/mood" style={{ font: '500 13px/1 var(--font-body)', color: 'var(--accent)', textDecoration: 'none', marginTop: '6px', display: 'inline-block' }}>🎭 Open Mood Tracker →</Link>
          </div>
        </div>
      </div>

      <div className={styles.content}>
        <motion.div className={styles.statsStrip} variants={container} initial="initial" animate="animate">
          {STATS.map(s => (
            <motion.div key={s.label} className={styles.statCard} variants={fadeUp}>
              <div className={styles.statValue}>{s.value}</div>
              <div className={styles.statLabel}>{s.label}</div>
            </motion.div>
          ))}
        </motion.div>

        <div style={{ display: 'flex', flexDirection: 'column', gap: 24 }}>
          {/* Knowledge Tree */}
          <motion.div className={styles.treeCard} variants={fadeUp} initial="initial" animate="animate" transition={{ delay: 0.2 }}>
            <div className={styles.sectionTitle}>📚 Knowledge Tree</div>
            <div className={styles.tree}>
              {TREE_NODES.map((node, idx) => (
                <motion.div
                  key={node.id}
                  className={styles.branch}
                  style={{ left: node.x, top: node.y, transform: 'translate(-50%, -50%)' }}
                  initial={{ opacity: 0, scale: 0 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ delay: 0.3 + idx * 0.07, type: 'spring', stiffness: 260, damping: 18 }}
                  title={`${node.label}: ${node.books} books`}
                >
                  <motion.div
                    className={styles.leaf}
                    style={{ width: node.size, height: node.size, background: `${node.color}18`, borderColor: `${node.color}44` }}
                    whileHover={{ scale: 1.2, borderColor: node.color }}
                    transition={{ type: 'spring', stiffness: 400, damping: 15 }}
                  >
                    {node.icon}
                  </motion.div>
                  <span className={styles.leafLabel}>{node.label}<br />{node.books} books</span>
                </motion.div>
              ))}
            </div>
          </motion.div>

          {/* Reading Bingo */}
          <motion.div className={styles.bingoCard} variants={fadeUp} initial="initial" animate="animate" transition={{ delay: 0.35 }}>
            <div className={styles.sectionTitle}>🎯 Reading Bingo 2026</div>
            <div className={styles.bingoGrid}>
              {BINGO_CELLS.map((cell, i) => (
                <div
                  key={i}
                  className={`${styles.bingoCell} ${completed.has(i) ? styles.completed : ''} ${i === 12 ? styles.center : ''}`}
                  onClick={() => toggle(i)}
                  title={cell}
                >
                  {i === 12 ? '⭐' : cell}
                </div>
              ))}
            </div>
            <div className={styles.bingoProgress}>
              <span>{progress}/25</span>
              <div className={styles.bingoBar}>
                <div className={styles.bingoFill} style={{ width: `${pct}%` }}></div>
              </div>
              <span>{pct}%</span>
            </div>
          </motion.div>
        </div>

        <motion.div style={{ display: 'flex', flexDirection: 'column', gap: 24 }} variants={container} initial="initial" animate="animate">
          <motion.div variants={fadeUp}>
            <AvatarBuilder />
          </motion.div>

          <motion.div className={styles.sideCard} variants={fadeUp}>
            <div className={styles.sectionTitle} style={{ margin: 0 }}>📖 Currently Reading</div>
            <div className={styles.currentBook}>
              <div className={styles.miniCover} style={{ background: 'linear-gradient(145deg, #b0622a, #d4845e, #f3a863)' }}></div>
              <div className={styles.bookDetails}>
                <div className={styles.bookTitle}>Dune</div>
                <div className={styles.bookAuthor}>Frank Herbert</div>
                <div className={styles.progressLabel}>
                  <span>Page 342 of 896</span>
                  <span>38%</span>
                </div>
                <div className={styles.progressBar}>
                  <div className={styles.progressFill} style={{ width: '38%' }}></div>
                </div>
              </div>
            </div>
          </motion.div>

          <motion.div className={styles.sideCard} variants={fadeUp}>
            <div className={styles.sectionTitle} style={{ margin: 0 }}>🏅 Badges</div>
            <div className={styles.badgesGrid}>
              {BADGES.map(b => (
                <div key={b.name} className={`${styles.badgeItem} ${b.locked ? styles.locked : ''}`} title={b.name}>
                  <span className={styles.badgeIcon}>{b.icon}</span>
                  <span className={styles.badgeName}>{b.name}</span>
                </div>
              ))}
            </div>
          </motion.div>
        </motion.div>
      </div>
    </div>
  );
}
