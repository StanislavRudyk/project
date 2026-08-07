import { Header } from '../../components/Header';
import { motion } from 'framer-motion';
import styles from './Leaderboard.module.css';

const TOP_READERS = [
    { rank: 1, initials: 'NK', name: 'Nora Kline', books: 47, reviews: 39, streak: 28, badge: '🏆', color: '#F59E0B' },
    { rank: 2, initials: 'AR', name: 'Asha Rao',   books: 41, reviews: 35, streak: 21, badge: '🥈', color: '#94A3B8' },
    { rank: 3, initials: 'MJ', name: 'Mark Jensen', books: 38, reviews: 31, streak: 14, badge: '🥉', color: '#CD7C2F' },
    { rank: 4, initials: 'LS', name: 'Liam Sørensen', books: 34, reviews: 29, streak: 9,  badge: null, color: '#4F46E5' },
    { rank: 5, initials: 'EV', name: 'Elena Vasquez', books: 30, reviews: 26, streak: 7,  badge: null, color: '#059669' },
    { rank: 6, initials: 'TK', name: 'Tomáš Kovář',   books: 28, reviews: 24, streak: 5,  badge: null, color: '#DC2626' },
    { rank: 7, initials: 'PB', name: 'Priya Bhat',    books: 25, reviews: 22, streak: 3,  badge: null, color: '#BE185D' },
    { rank: 8, initials: 'JO', name: 'James O\'Brien', books: 22, reviews: 18, streak: 2, badge: null, color: '#0369A1' },
];

const podiumOrder = [TOP_READERS[1], TOP_READERS[0], TOP_READERS[2]]; // 2nd, 1st, 3rd

const container = { animate: { transition: { staggerChildren: 0.05 } } };
const rowItem = {
    initial: { opacity: 0, x: -16 },
    animate: { opacity: 1, x: 0, transition: { duration: 0.35, ease: [0.22, 1, 0.36, 1] } },
};

export default function Leaderboard() {
    return (
        <div className={styles.page}>
            <Header />

            <div className={styles.hero}>
                <div className={styles.kicker}><span></span> Weekly Rankings</div>
                <h1 className={styles.heroTitle}>Leaderboard</h1>
                <p className={styles.heroSub}>The most dedicated readers this week. Will you make the list?</p>
            </div>

            <div className={styles.content}>
                {/* Podium */}
                <div className={styles.podium}>
                    {podiumOrder.map((reader) => (
                        <motion.div
                            key={reader.rank}
                            className={`${styles.podiumCard} ${reader.rank === 1 ? styles.first : ''}`}
                            initial={{ opacity: 0, y: 30 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ delay: reader.rank === 1 ? 0.2 : reader.rank === 2 ? 0 : 0.1, duration: 0.5, ease: [0.22, 1, 0.36, 1] }}
                        >
                            {reader.badge && <span className={styles.podiumBadge}>{reader.badge}</span>}
                            <div className={styles.podiumAvatar} style={{ background: reader.color, width: reader.rank === 1 ? 68 : 52, height: reader.rank === 1 ? 68 : 52, fontSize: reader.rank === 1 ? 24 : 18 }}>
                                {reader.initials}
                            </div>
                            <div className={styles.podiumName}>{reader.name}</div>
                            <div className={styles.podiumStats}>
                                <div className={styles.podiumStat}><strong>{reader.books}</strong> books</div>
                                <div className={styles.podiumStat}><strong>{reader.reviews}</strong> reviews</div>
                            </div>
                            <div className={styles.podiumRank}>#{reader.rank}</div>
                        </motion.div>
                    ))}
                </div>

                {/* Full list */}
                <div>
                    <h2 className={styles.sectionTitle}>Full Rankings</h2>
                    <motion.div className={styles.list} variants={container} initial="initial" animate="animate">
                        {TOP_READERS.map(reader => (
                            <motion.div key={reader.rank} className={styles.listItem} variants={rowItem}>
                                <div className={styles.rank}>
                                    {reader.badge ? <span className={styles.badge}>{reader.badge}</span> : `#${reader.rank}`}
                                </div>
                                <div className={styles.listAvatar} style={{ background: reader.color }}>{reader.initials}</div>
                                <div className={styles.listInfo}>
                                    <div className={styles.listName}>{reader.name}</div>
                                    <div className={styles.streakBar}>
                                        {Array.from({ length: 7 }).map((_, i) => (
                                            <div key={i} className={`${styles.streakDot} ${i < Math.min(reader.streak / 4, 7) ? styles.active : ''}`}></div>
                                        ))}
                                    </div>
                                </div>
                                <div className={styles.listScore}>
                                    <span className={styles.scoreValue}>{reader.books}</span>
                                    <span className={styles.scoreLabel}>books read</span>
                                </div>
                            </motion.div>
                        ))}
                    </motion.div>
                </div>
            </div>
        </div>
    );
}
