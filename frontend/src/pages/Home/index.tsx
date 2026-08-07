import styles from './Home.module.css';
import { Header } from '../../components/Header';
import { useState, useEffect, useCallback, useRef } from 'react';
import { motion, useScroll, useTransform } from 'framer-motion';
import { AchievementToast, useAchievements, ACHIEVEMENTS } from '../../components/AchievementToast';
import { QuoteSharer } from '../../components/QuoteSharer';
import { MagneticButton } from '../../components/MagneticButton';
import { PollCard } from '../../components/PollCard';

const SkeletonCard = () => (
    <article className={styles.card}>
        <div className={styles.cardTop}>
            <div className={styles.skeletonCover}></div>
            <div className={styles.cardBook} style={{ flex: 1 }}>
                <div className={styles.skeletonText} style={{ width: '80%', height: '24px', marginBottom: '8px' }}></div>
                <div className={styles.skeletonText} style={{ width: '50%', height: '16px' }}></div>
            </div>
        </div>
        <div className={styles.skeletonText} style={{ width: '100%', height: '14px', marginTop: '16px' }}></div>
        <div className={styles.skeletonText} style={{ width: '100%', height: '14px', marginTop: '8px' }}></div>
        <div className={styles.skeletonText} style={{ width: '60%', height: '14px', marginTop: '8px' }}></div>
        
        <div className={styles.userRow} style={{ marginTop: '20px' }}>
            <div className={styles.skeletonAvatar}></div>
            <div className={styles.skeletonText} style={{ width: '100px', height: '14px' }}></div>
        </div>
    </article>
);

const ReviewCard = ({ bookTitle, author, snippet, userInitials, userName, bgAvatar, bgCover, onAchievement }: any) => {
    const [isLiked, setIsLiked] = useState(false);
    const [isSaved, setIsSaved] = useState(false);
    const [quoteText, setQuoteText] = useState<string | null>(null);
    const likedOnce = useRef(false);
    const savedOnce = useRef(false);

    const handleLike = () => {
        setIsLiked(!isLiked);
        if (!likedOnce.current) { likedOnce.current = true; onAchievement?.(ACHIEVEMENTS.FIRST_LIKE); }
    };
    const handleSave = () => {
        setIsSaved(!isSaved);
        if (!savedOnce.current) { savedOnce.current = true; onAchievement?.(ACHIEVEMENTS.FIRST_SAVE); }
    };
    const handleTextSelect = () => {
        const sel = window.getSelection()?.toString().trim();
        if (sel && sel.length > 20) setQuoteText(sel);
    };

    return (
        <>
        <motion.article 
            className={styles.card}
            whileHover={{ y: -8 }}
            transition={{ type: "spring", stiffness: 300, damping: 20 }}
        >
            <div className={styles.cardTop}>
                <motion.div 
                    className={styles.miniCover} 
                    style={{ background: bgCover }} 
                    aria-hidden="true"
                    whileHover={{ rotateY: 15, rotateX: -5, scale: 1.05 }}
                    transition={{ type: "spring", stiffness: 400, damping: 15 }}
                ></motion.div>
                <div className={styles.cardBook}>
                    <h3>{bookTitle}</h3>
                    <p className={styles.author}>{author}</p>
                </div>
            </div>
            <p className={styles.snippet} onMouseUp={handleTextSelect} title="Select text to share a quote">{snippet}</p>
            <div className={styles.userRow}>
                <div className={styles.avatar} style={{ background: bgAvatar }}>{userInitials}</div>
                <div className={styles.userMeta}>
                    <div className={styles.name}>{userName}</div>
                </div>
            </div>
            <div className={styles.actions}>
                <button 
                    className={`${styles.actionBtn} ${isLiked ? styles.liked : ''}`}
                    onClick={handleLike}
                >
                    <svg width="16" height="16" viewBox="0 0 24 24" fill={isLiked ? "currentColor" : "none"} stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
                    </svg>
                    {isLiked ? 129 : 128}
                </button>
                <button 
                    className={`${styles.actionBtn} ${isSaved ? styles.saved : ''}`}
                    onClick={handleSave}
                >
                    <svg width="16" height="16" viewBox="0 0 24 24" fill={isSaved ? "currentColor" : "none"} stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M19 21l-7-5-7 5V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2z"></path>
                    </svg>
                    {isSaved ? 'Saved' : 'Save'}
                </button>
                <button
                    className={styles.actionBtn}
                    onClick={() => setQuoteText(snippet)}
                    title="Share a quote from this review"
                >
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"></path>
                    </svg>
                    Quote
                </button>
            </div>
        </motion.article>
        {quoteText && (
            <QuoteSharer
                quote={quoteText}
                bookTitle={bookTitle}
                author={author}
                onClose={() => setQuoteText(null)}
            />
        )}
        </>
    );
};

const BOOKS_OF_DAY = [
    { title: 'The Salt House', author: 'Mira Ellison', pub: 'River & Reed, 2024', genre: 'Literary Fiction', coverBg: 'linear-gradient(145deg,#1a3a2f,#2d6a4f,#74c69d)' },
    { title: 'Orbital', author: 'Samantha Harvey', pub: 'Jonathan Cape, 2023', genre: 'Literary Fiction', coverBg: 'linear-gradient(145deg,#1e3a5f,#3d7ea6,#f0c27a)' },
    { title: 'Dune', author: 'Frank Herbert', pub: 'Chilton Books, 1965', genre: 'Sci-Fi', coverBg: 'linear-gradient(145deg,#b0622a,#d4845e,#f3a863)' },
    { title: 'The Creative Act', author: 'Rick Rubin', pub: 'Penguin Press, 2023', genre: 'Non-Fiction', coverBg: 'linear-gradient(145deg,#d3c4b1,#c0b090,#e3dfda)' },
];

export default function Home() {
    const [activeCategory, setActiveCategory] = useState('All');
    const [isLoading, setIsLoading] = useState(true);
    const [bookIdx, setBookIdx] = useState(0);
    const currentBook = BOOKS_OF_DAY[bookIdx];
    const { current: achievement, unlock, dismiss } = useAchievements();
    const luckyUsed = useRef(false);

    const handleRandom = useCallback(() => {
        let next = Math.floor(Math.random() * BOOKS_OF_DAY.length);
        if (next === bookIdx) next = (bookIdx + 1) % BOOKS_OF_DAY.length;
        setBookIdx(next);
        if (!luckyUsed.current) { luckyUsed.current = true; unlock(ACHIEVEMENTS.LUCKY_READER); }
    }, [bookIdx, unlock]);
    
    // Parallax effect for Hero
    const { scrollY } = useScroll();
    const yHero = useTransform(scrollY, [0, 500], [0, 150]);
    const opacityHero = useTransform(scrollY, [0, 300], [1, 0]);

    useEffect(() => {
        // Simulate network loading
        const timer = setTimeout(() => setIsLoading(false), 1500);
        return () => clearTimeout(timer);
    }, []);
    

    return (
        <>
        <AchievementToast achievement={achievement} onClose={dismiss} />
        <div 
            style={{ 
                transition: 'background 1.2s ease',
                background: `radial-gradient(ellipse 60% 40% at 20% 10%, ${currentBook.coverBg.match(/#[a-f0-9]{6}/gi)?.[0] ?? 'transparent'}22, transparent 60%)`,
                minHeight: '100vh',
            }}
        >
        <Header />
            <main className={styles.main}>
                <motion.section 
                    className={styles.hero}
                    style={{ y: yHero, opacity: opacityHero }}
                >
                    <div className={styles.heroContent}>
                        <div className={`${styles.heroGlow} ${styles.glowG1}`} aria-hidden="true"></div>
                        <div className={`${styles.heroGlow} ${styles.glowG2}`} aria-hidden="true"></div>
                        <div className={`${styles.heroGlow} ${styles.glowG3}`} aria-hidden="true"></div>

                        <div className={styles.kicker}><span></span> Book of the Day</div>

                        <div className={styles.heroStage}>
                            <div className={styles.coverWrap}>
                                <div className={styles.coverGlow} aria-hidden="true"></div>
                                <motion.article 
                                    key={currentBook.title}
                                    className={styles.cover} 
                                    aria-hidden="true"
                                    initial={{ rotateY: 90, opacity: 0 }}
                                    animate={{ rotateY: 0, opacity: 1 }}
                                    transition={{ duration: 0.5, ease: [0.22, 1, 0.36, 1] }}
                                    style={{ background: currentBook.coverBg }}
                                >
                                    <div className={styles.spineLabel}>{currentBook.genre}</div>
                                    <h2 className={styles.coverTitle}>{currentBook.title}</h2>
                                </motion.article>
                            </div>

                            <div className={styles.heroMeta}>
                                <h1 id="botd-title">{currentBook.title}</h1>
                                <p className={styles.by}>{currentBook.author} · {currentBook.pub}</p>
                                
                                <div className={styles.heroActions}>
                                    <button type="button" className={styles.btnPrimary}>Read reviews</button>
                                    <button 
                                        type="button" 
                                        className={styles.btnSecondary}
                                        onClick={handleRandom}
                                        title="Show a random book"
                                    >🎲 Lucky Pick</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </motion.section>

                <section className={styles.feed}>
                    <h2 className={styles.feedTitle}>Community Feed</h2>
                    
                    <div className={styles.categories}>
                        {['All', 'Fiction', 'Non-Fiction', 'Sci-Fi', 'Biography', 'Design'].map(cat => (
                            <button 
                                key={cat}
                                className={`${styles.catBtn} ${activeCategory === cat ? styles.active : ''}`}
                                onClick={() => setActiveCategory(cat)}
                            >
                                {cat}
                            </button>
                        ))}
                    </div>

                    <div className={styles.masonry}>
                        {isLoading ? (
                            <>
                                <SkeletonCard />
                                <SkeletonCard />
                                <SkeletonCard />
                                <SkeletonCard />
                            </>
                        ) : (
                            <>
                                <ReviewCard 
                                    bookTitle="Orbital" 
                                    author="Samantha Harvey" 
                                    snippet="Sixteen orbits, one day — and somehow it made me miss Earth more than any travel essay ever has." 
                                    userInitials="NK" 
                                    userName="Nora K." 
                                    bgCover="linear-gradient(160deg, #1e3a5f, #3d7ea6 50%, #f0c27a)"
                                    bgAvatar="var(--muted)"
                                    onAchievement={unlock}
                                />
                                <ReviewCard 
                                    bookTitle="Tomorrow, and Tomorrow…" 
                                    author="Gabrielle Zevin" 
                                    snippet="Friendship as a creative medium. I kept underlining lines about making something together." 
                                    userInitials="AR" 
                                    userName="Asha R." 
                                    bgCover="linear-gradient(150deg, #3b1f2b, #8b4557 55%, #e8b4a0)"
                                    bgAvatar="#9A3412"
                                    onAchievement={unlock}
                                />
                                <PollCard
                                    question="What should be our Book of the Month for August?"
                                    initialOptions={[
                                      { id: '1', text: 'Project Hail Mary (Andy Weir)', votes: 142 },
                                      { id: '2', text: 'Babel (R.F. Kuang)', votes: 89 },
                                      { id: '3', text: 'Piranesi (Susanna Clarke)', votes: 215 },
                                    ]}
                                />
                                <ReviewCard 
                                    bookTitle="The Creative Act"  
                                    author="Rick Rubin" 
                                    snippet="A beautiful meditation on the creative process. It feels less like a book and more like a gentle push." 
                                    userInitials="MJ" 
                                    userName="Mark J." 
                                    bgCover="linear-gradient(120deg, #d3c4b1, #e3dfda)"
                                    bgAvatar="#4F46E5"
                                    onAchievement={unlock}
                                />
                                <ReviewCard 
                                    bookTitle="Dune" 
                                    author="Frank Herbert" 
                                    snippet="I finally read it. The world-building is unmatched, and the political intrigue is brilliant." 
                                    userInitials="LS" 
                                    userName="Liam S." 
                                    bgCover="linear-gradient(160deg, #b0622a, #f3a863)"
                                    bgAvatar="#E11D48"
                                    onAchievement={unlock}
                                />
                            </>
                        )}
                    </div>
                </section>
            </main>
        </div>
        </>
    )
}
