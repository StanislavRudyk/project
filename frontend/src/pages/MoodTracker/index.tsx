import { useState } from 'react';
import { Header } from '../../components/Header';
import { motion } from 'framer-motion';
import styles from './MoodTracker.module.css';

const BOOKS = ['Dune', 'The Salt House', 'Orbital'];

const MOODS = [
  { emoji: '😢', label: 'Sad',       color: '#3B82F6', value: 1 },
  { emoji: '😰', label: 'Tense',     color: '#8B5CF6', value: 2 },
  { emoji: '😐', label: 'Neutral',   color: '#94A3B8', value: 3 },
  { emoji: '🤔', label: 'Curious',   color: '#F59E0B', value: 4 },
  { emoji: '😊', label: 'Happy',     color: '#10B981', value: 5 },
  { emoji: '😭', label: 'Emotional', color: '#EC4899', value: 6 },
  { emoji: '😱', label: 'Shocked',   color: '#EF4444', value: 7 },
];

const CHAPTERS = [
  { num: 1,  title: 'Chapter 1 — Arrival',      defaultMood: 3 },
  { num: 2,  title: 'Chapter 2 — The Warning',   defaultMood: 2 },
  { num: 3,  title: 'Chapter 3 — Desert Power',  defaultMood: 5 },
  { num: 4,  title: 'Chapter 4 — Betrayal',      defaultMood: 7 },
  { num: 5,  title: 'Chapter 5 — Exile',         defaultMood: 1 },
  { num: 6,  title: 'Chapter 6 — The Fremen',    defaultMood: 4 },
  { num: 7,  title: 'Chapter 7 — Awakening',     defaultMood: 6 },
];

const MOOD_COLORS: Record<number, string> = {};
MOODS.forEach(m => { MOOD_COLORS[m.value] = m.color; });

const fadeUp = {
  initial: { opacity: 0, y: 18 },
  animate: { opacity: 1, y: 0, transition: { duration: 0.38, ease: [0.22, 1, 0.36, 1] } },
};

export default function MoodTracker() {
  const [activeBook, setActiveBook]   = useState('Dune');
  const [moods, setMoods]             = useState<Record<number, number>>(() =>
    Object.fromEntries(CHAPTERS.map(c => [c.num, c.defaultMood]))
  );
  const [notes, setNotes]             = useState<Record<number, string>>({});

  const setMood = (chap: number, val: number) =>
    setMoods(prev => ({ ...prev, [chap]: val }));

  const setNote = (chap: number, val: string) =>
    setNotes(prev => ({ ...prev, [chap]: val }));

  return (
    <div className={styles.page}>
      <Header />

      <div className={styles.hero}>
        <div className={styles.kicker}><span></span> Mood Journal</div>
        <h1 className={styles.heroTitle}>Emotional Tracker</h1>
        <p className={styles.heroSub}>Track how each chapter makes you feel and discover your reading patterns.</p>
      </div>

      <div className={styles.content}>
        {/* Left: timeline */}
        <div>
          <div className={styles.bookSelect}>
            <div className={styles.sectionTitle}>Select Book</div>
            <div className={styles.bookPills}>
              {BOOKS.map(b => (
                <button key={b} className={`${styles.pill} ${activeBook === b ? styles.active : ''}`} onClick={() => setActiveBook(b)}>
                  {b}
                </button>
              ))}
            </div>
          </div>

          <div className={styles.timeline} style={{ marginTop: 24 }}>
            {CHAPTERS.map((ch) => (
              <motion.div key={ch.num} className={styles.chapterRow} variants={fadeUp} initial="initial" animate="animate" transition={{ delay: ch.num * 0.05 }}>
                <div className={styles.chapterNum}>{ch.num}</div>
                <div className={styles.chapterInfo}>
                  <div className={styles.chapterTitle}>{ch.title}</div>
                  <div className={styles.moodPicker}>
                    {MOODS.map(mood => (
                      <button
                        key={mood.value}
                        className={`${styles.moodBtn} ${moods[ch.num] === mood.value ? styles.selected : ''}`}
                        onClick={() => setMood(ch.num, mood.value)}
                        title={mood.label}
                        style={moods[ch.num] === mood.value ? { borderColor: mood.color, background: `${mood.color}15` } : {}}
                      >
                        {mood.emoji}
                      </button>
                    ))}
                  </div>
                  <textarea
                    className={styles.moodNote}
                    rows={2}
                    placeholder="Add a note about this chapter..."
                    value={notes[ch.num] ?? ''}
                    onChange={e => setNote(ch.num, e.target.value)}
                  />
                </div>
              </motion.div>
            ))}
          </div>
        </div>

        {/* Right sidebar: chart */}
        <motion.div style={{ display: 'flex', flexDirection: 'column', gap: 24 }} initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.3 }}>
          <div className={styles.chartCard}>
            <div className={styles.sectionTitle} style={{ margin: '0 0 16px' }}>📊 Mood Graph</div>
            <div className={styles.chart}>
              {CHAPTERS.map((ch) => {
                const val = moods[ch.num] ?? 3;
                const pct = ((val - 1) / 6) * 100;
                const color = MOOD_COLORS[val] ?? '#94A3B8';
                return (
                  <div
                    key={ch.num}
                    className={styles.bar}
                    style={{ height: `${pct}%`, background: color, opacity: 0.85 }}
                  >
                    <span className={styles.barTooltip}>Ch.{ch.num}: {MOODS.find(m => m.value === val)?.label}</span>
                  </div>
                );
              })}
            </div>
            <div className={styles.chartLabels}>
              {CHAPTERS.map(ch => (
                <span key={ch.num} className={styles.chartLabel}>{ch.num}</span>
              ))}
            </div>
            <div className={styles.legendGrid}>
              {MOODS.map(m => (
                <div key={m.value} className={styles.legendItem}>
                  <div className={styles.legendDot} style={{ background: m.color }}></div>
                  {m.emoji} {m.label}
                </div>
              ))}
            </div>
          </div>
        </motion.div>
      </div>
    </div>
  );
}
