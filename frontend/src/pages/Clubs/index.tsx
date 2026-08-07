import { useState } from 'react';
import { Header } from '../../components/Header';
import { motion } from 'framer-motion';
import styles from './Clubs.module.css';

const CLUBS = [
    { id: 1, icon: '🌌', name: 'Sci-Fi Collective', genre: 'Sci-Fi', desc: 'Exploring galaxies, futures and dystopias. We read one hard sci-fi book per month and dive deep.', members: 1842, colors: ['#4F46E5', '#818CF8', '#6366F1'], joined: false },
    { id: 2, icon: '🌿', name: 'The Quiet Page', genre: 'Literary Fiction', desc: 'Slow readers who appreciate beautiful prose over plot twists. No spoilers, ever.', members: 963, colors: ['#059669', '#34D399', '#10B981'], joined: false },
    { id: 3, icon: '🔪', name: 'Crime & Clues', genre: 'Mystery/Thriller', desc: 'Whodunits, psychological thrillers, and true crime. We vote on the book every month.', members: 2341, colors: ['#DC2626', '#F87171', '#EF4444'], joined: true },
    { id: 4, icon: '🏛️', name: 'Non-Fiction Society', genre: 'Non-Fiction', desc: 'From history to neuroscience — books that change how you see the world.', members: 1207, colors: ['#92400E', '#D97706', '#F59E0B'], joined: false },
    { id: 5, icon: '🌸', name: 'Contemporary Romance', genre: 'Romance', desc: 'Feel-good reads with complex characters and satisfying endings. All are welcome here.', members: 3102, colors: ['#BE185D', '#EC4899', '#F472B6'], joined: false },
    { id: 6, icon: '🏔️', name: 'Adventure & Travel', genre: 'Adventure', desc: 'Books that make you want to pack a bag immediately. Memoirs, travel essays, survival stories.', members: 784, colors: ['#0369A1', '#0EA5E9', '#38BDF8'], joined: false },
];

const GENRES = ['All', 'Sci-Fi', 'Literary Fiction', 'Mystery/Thriller', 'Non-Fiction', 'Romance', 'Adventure'];

const container = {
    animate: { transition: { staggerChildren: 0.06 } },
};
const item = {
    initial: { opacity: 0, y: 20 },
    animate: { opacity: 1, y: 0, transition: { duration: 0.4, ease: [0.22, 1, 0.36, 1] } },
};

export default function Clubs() {
    const [filter, setFilter] = useState('All');
    const [clubs, setClubs] = useState(CLUBS);

    const visible = filter === 'All' ? clubs : clubs.filter(c => c.genre === filter);

    const toggle = (id: number) =>
        setClubs(prev => prev.map(c => c.id === id ? { ...c, joined: !c.joined } : c));

    return (
        <div className={styles.page}>
            <Header />

            <div className={styles.hero}>
                <div className={styles.kicker}><span></span> Community</div>
                <h1 className={styles.heroTitle}>Book Clubs</h1>
                <p className={styles.heroSub}>Find your people. Every great book is better shared with someone who gets it.</p>
                <div className={styles.heroActions} style={{ marginTop: '28px' }}>
                    <button className={styles.btnPrimary}>Create a Club</button>
                    <button className={styles.btnSecondary}>Browse All</button>
                </div>
            </div>

            <div className={styles.content}>
                <div className={styles.filterBar}>
                    {GENRES.map(g => (
                        <button key={g} className={`${styles.filterBtn} ${filter === g ? styles.active : ''}`} onClick={() => setFilter(g)}>{g}</button>
                    ))}
                </div>

                <motion.div className={styles.grid} variants={container} initial="initial" animate="animate">
                    {visible.map(club => (
                        <motion.div key={club.id} className={styles.card} variants={item}>
                            <div className={styles.cardHeader}>
                                <div className={styles.clubIcon} style={{ background: `linear-gradient(135deg, ${club.colors[0]}, ${club.colors[1]})` }}>
                                    {club.icon}
                                </div>
                                <div>
                                    <div className={styles.clubName}>{club.name}</div>
                                    <div className={styles.clubGenre}>{club.genre}</div>
                                </div>
                            </div>
                            <p className={styles.clubDesc}>{club.desc}</p>
                            <div className={styles.cardFooter}>
                                <div className={styles.memberCount}>
                                    <div className={styles.memberAvatars}>
                                        {club.colors.map((c, i) => (
                                            <div key={i} className={styles.memberAvatar} style={{ background: c }}>
                                                {String.fromCharCode(65 + i)}
                                            </div>
                                        ))}
                                    </div>
                                    <span>{club.members.toLocaleString()} members</span>
                                </div>
                                <button
                                    className={`${styles.joinBtn} ${club.joined ? styles.joined : ''}`}
                                    onClick={() => toggle(club.id)}
                                >
                                    {club.joined ? '✓ Joined' : 'Join'}
                                </button>
                            </div>
                        </motion.div>
                    ))}
                </motion.div>
            </div>
        </div>
    );
}
