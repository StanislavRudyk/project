import { useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { motion } from 'framer-motion';
import { Header } from '../../components/Header';
import { MagneticButton } from '../../components/MagneticButton';
import styles from './BookDetail.module.css';

const BOOKS: Record<string, any> = {
  'orbital': {
    title: 'Orbital',
    author: 'Samantha Harvey',
    pub: 'Jonathan Cape, 2023',
    genre: 'Literary Fiction',
    pages: 209,
    coverBg: 'linear-gradient(145deg,#1e3a5f,#3d7ea6,#f0c27a)',
    desc: 'A day in the life of six astronauts aboard the International Space Station. One rotation of the Earth. Sixteen sunrises. This is a novel about beauty, vulnerability, and what it means to be human when you can see the whole planet at once.',
    rating: 4.6,
    reviews: [
      { name: 'Nora K.', initials: 'NK', color: '#4F46E5', stars: 5, date: 'Jul 2024', text: 'Sixteen orbits, one day — and somehow it made me miss Earth more than any travel essay ever has. Harvey\'s prose is unlike anything I\'ve read.' },
      { name: 'Mark J.',  initials: 'MJ', color: '#059669', stars: 4, date: 'Jun 2024', text: 'Meditative and beautiful. Not a plot-driven book, but if you love introspective literary fiction, this is a masterpiece.' },
      { name: 'Liam S.', initials: 'LS', color: '#DC2626', stars: 5, date: 'May 2024', text: 'Won the Booker for a reason. The descriptions of Earth from orbit are some of the most moving passages I\'ve ever read.' },
    ],
  },
  'dune': {
    title: 'Dune',
    author: 'Frank Herbert',
    pub: 'Chilton Books, 1965',
    genre: 'Sci-Fi',
    pages: 896,
    coverBg: 'linear-gradient(145deg,#b0622a,#d4845e,#f3a863)',
    desc: 'Set in the distant future amidst a feudal interstellar society, Dune tells the story of young Paul Atreides, whose family accepts stewardship of the desert planet Arrakis — the only source of the spice melange, the most precious substance in the universe.',
    rating: 4.8,
    reviews: [
      { name: 'Asha R.', initials: 'AR', color: '#9A3412', stars: 5, date: 'Jul 2024', text: 'The world-building is completely unmatched. I\'ve never felt more immersed in a fictional universe.' },
      { name: 'Elena V.', initials: 'EV', color: '#7C3AED', stars: 5, date: 'Jun 2024', text: 'A political and philosophical masterpiece wrapped inside a sci-fi adventure. Essential reading.' },
    ],
  },
};

const DEFAULT_BOOK = {
  title: 'The Salt House', author: 'Mira Ellison', pub: 'River & Reed, 2024', genre: 'Literary Fiction',
  pages: 312, coverBg: 'linear-gradient(145deg,#1a3a2f,#2d6a4f,#74c69d)',
  desc: 'A quiet, devastating novel about a woman returning to her childhood home on a Scottish island after her mother\'s death. A story about grief, memory, and the sea.',
  rating: 4.4,
  reviews: [{ name: 'Jane D.', initials: 'JD', color: '#F59E0B', stars: 4, date: 'Aug 2024', text: 'Beautifully atmospheric. The island feels completely alive.' }],
};

function TimeToRead({ pages, pagesPerHour }: { pages: number; pagesPerHour: number }) {
  const hours = pages / pagesPerHour;
  const h = Math.floor(hours);
  const m = Math.round((hours - h) * 60);
  return (
    <div style={{ fontWeight: 700, fontSize: 28, lineHeight: 1, color: 'var(--fg)', letterSpacing: '-0.03em' }}>
      {h > 0 ? `${h}h ` : ''}{m > 0 ? `${m}m` : ''}
    </div>
  );
}

const fadeUp = {
  initial: { opacity: 0, y: 20 },
  animate: { opacity: 1, y: 0, transition: { duration: 0.4, ease: [0.22, 1, 0.36, 1] } },
};

export default function BookDetail() {
  const { id } = useParams<{ id: string }>();
  const book = BOOKS[id ?? ''] ?? DEFAULT_BOOK;
  const [pagesPerHour, setPagesPerHour] = useState(40);
  const [myRating, setMyRating]         = useState(0);
  const [reviewText, setReviewText]     = useState('');
  const [isAnonymous, setIsAnonymous]   = useState(false);

  return (
    <div className={styles.page}>
      <Header />

      {/* Hero with ambient glow */}
      <div className={styles.hero}>
        <div className={styles.heroGlow} style={{ width: 400, height: 400, top: -100, right: -60, background: book.coverBg.match(/#[a-f0-9]{6}/gi)?.[1] ?? '#3d7ea6', opacity: 0.12 }} />
        <div className={styles.heroGlow} style={{ width: 280, height: 280, bottom: -80, left: 40, background: book.coverBg.match(/#[a-f0-9]{6}/gi)?.[0] ?? '#1e3a5f', opacity: 0.08 }} />
        <div className={styles.heroContent}>
          <div className={styles.kicker}><span></span> {book.genre}</div>
          <h1 className={styles.heroTitle}>{book.title}</h1>
          <p className={styles.heroAuthor}>{book.author} · {book.pub}</p>
          <div className={styles.metaRow}>
            <div className={styles.metaChip}>
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"/><path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"/></svg>
              {book.pages} pages
            </div>
            <div className={styles.metaChip}>
              ⭐ {book.rating} / 5
            </div>
            <div className={styles.metaChip}>
              💬 {book.reviews.length} reviews
            </div>
            <Link to="/mood" className={styles.metaChip} style={{ textDecoration: 'none' }}>
              🎭 Mood Tracker
            </Link>
          </div>
        </div>
      </div>

      <div className={styles.content}>
        {/* LEFT: cover + TTR */}
        <motion.div className={styles.coverCard} variants={fadeUp} initial="initial" animate="animate">
          <motion.div
            className={styles.cover}
            style={{ background: book.coverBg }}
            whileHover={{ rotateY: 8, rotateX: -4, scale: 1.02 }}
            transition={{ type: 'spring', stiffness: 300, damping: 20 }}
          >
            <div className={styles.coverGenre}>{book.genre}</div>
            <div className={styles.coverTitle}>{book.title}</div>
          </motion.div>

          {/* Time To Read Calculator */}
          <div className={styles.ttrBox}>
            <div className={styles.ttrTitle}>⏱ Time to Read</div>
            <TimeToRead pages={book.pages} pagesPerHour={pagesPerHour} />
            <div className={styles.ttrSub}>at your reading speed</div>
            <div className={styles.ttrSlider}>
              <div className={styles.sliderLabel}>
                <span>Pages per hour</span>
              </div>
              <div className={styles.sliderRow}>
                <input
                  type="range"
                  className={styles.slider}
                  min={10}
                  max={100}
                  value={pagesPerHour}
                  onChange={e => setPagesPerHour(Number(e.target.value))}
                />
                <span className={styles.sliderValue}>{pagesPerHour} p/h</span>
              </div>
            </div>
          </div>

          <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
            <MagneticButton variant="accent" style={{ width: '100%' }}>Add to My List</MagneticButton>
            <MagneticButton variant="primary" style={{ width: '100%' }}>Write a Review</MagneticButton>
          </div>
        </motion.div>

        {/* RIGHT: description + reviews */}
        <div className={styles.reviewsSection}>
          <motion.p variants={fadeUp} initial="initial" animate="animate" style={{ font: '400 16px/1.7 var(--font-body)', color: 'var(--muted)', marginBottom: 32 }}>
            {book.desc}
          </motion.p>

          <div className={styles.sectionHead}>
            <div className={styles.sectionTitle}>Community Reviews</div>
            <div className={styles.ratingRow}>
              <div className={styles.stars}>
                {[1,2,3,4,5].map(n => (
                  <span key={n} className={styles.star}>
                    {n <= Math.round(book.rating) ? '★' : '☆'}
                  </span>
                ))}
              </div>
              <span className={styles.ratingNum}>{book.rating}</span>
              <span className={styles.ratingCount}>({book.reviews.length} reviews)</span>
            </div>
          </div>

          <div className={styles.reviewList}>
            {book.reviews.map((r: any, i: number) => (
              <motion.div
                key={i}
                className={styles.reviewCard}
                initial={{ opacity: 0, y: 16 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.1 + i * 0.08, duration: 0.4, ease: [0.22, 1, 0.36, 1] }}
              >
                <div className={styles.reviewHeader}>
                  <div className={styles.reviewAvatar} style={{ background: r.color }}>{r.initials}</div>
                  <div>
                    <div className={styles.reviewerName}>{r.name}</div>
                    <div className={styles.reviewStars}>
                      {[1,2,3,4,5].map(n => (
                        <span key={n} className={styles.reviewStar} style={{ color: n <= r.stars ? 'var(--accent)' : 'var(--border)' }}>★</span>
                      ))}
                    </div>
                  </div>
                  <span style={{ marginLeft: 'auto', font: '400 12px/1 var(--font-body)', color: 'var(--muted)' }}>{r.date}</span>
                </div>
                <p className={styles.reviewText}>{r.text}</p>
              </motion.div>
            ))}

            {/* Write review */}
            <motion.div className={styles.writeReview} initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.4 }}>
              <div className={styles.writeTitle}>Share your thoughts</div>
              <div className={styles.starPicker}>
                {[1,2,3,4,5].map(n => (
                  <span key={n} className={`${styles.starPick} ${n <= myRating ? styles.active : ''}`} onClick={() => setMyRating(n)}>★</span>
                ))}
              </div>
              <textarea
                className={styles.reviewInput}
                placeholder="What did you think of this book?"
                value={reviewText}
                onChange={e => setReviewText(e.target.value)}
              />
              <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginTop: 8 }}>
                <label style={{ display: 'flex', alignItems: 'center', gap: 8, cursor: 'pointer', font: '500 13px/1 var(--font-body)', color: 'var(--muted)' }}>
                  <input type="checkbox" checked={isAnonymous} onChange={e => setIsAnonymous(e.target.checked)} style={{ width: 16, height: 16, accentColor: 'var(--accent)' }} />
                  Post anonymously (Ghost Mode 👻)
                </label>
                <MagneticButton variant="accent">Post Review</MagneticButton>
              </div>
            </motion.div>
          </div>
        </div>
      </div>
    </div>
  );
}
