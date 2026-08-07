import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import styles from './Header.module.css';
import { ReadingStreak } from '../ReadingStreak';

export const Header = () => {
    const [isScrolled, setIsScrolled] = useState(false);
    const [isSearchOpen, setIsSearchOpen] = useState(false);
    const [theme, setTheme] = useState('dark'); // or read from context/localStorage
    const [isLoggedIn, setIsLoggedIn] = useState(true);
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);

    useEffect(() => {
        const handleScroll = () => {
            setIsScrolled(window.scrollY > 10);
        };
        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, []);

    const toggleTheme = () => setTheme(theme === 'dark' ? 'light' : 'dark');

    return (
        <header className={`${styles.header} ${isScrolled ? styles.scrolled : ''}`}>
                <Link to="/" className={styles.logo} aria-label="BookShare Home">
                    <svg width="28" height="28" viewBox="0 0 24 24" fill="none">
                        <path d="M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1 0-5H20" stroke="var(--accent)" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                    </svg>
                    <span>Pagefold</span>
                </Link>

                <div className={styles.search} onClick={() => setIsSearchOpen(true)}>
                    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden="true">
                        <circle cx="7" cy="7" r="4.5" stroke="currentColor" strokeWidth="1.5"/>
                        <path d="M10.5 10.5L14 14" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
                    </svg>
                    <span className={styles.searchFakeInput}>Search books, authors, lists…</span>
                    <span className={styles.searchShortcut}>⌘K</span>
                </div>

                <nav className={styles.navLinks}>
                    <Link to="/">Explore</Link>
                    <Link to="/clubs">Clubs</Link>
                    <Link to="/leaderboard">Rankings</Link>
                    <Link to="/debates">Debates</Link>
                    
                    <ReadingStreak days={7} />
                    
                    <button className={styles.iconBtn} onClick={toggleTheme} aria-label="Toggle Theme">
                        {theme === 'light' ? (
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"></path></svg>
                        ) : (
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><circle cx="12" cy="12" r="5"></circle><line x1="12" y1="1" x2="12" y2="3"></line><line x1="12" y1="21" x2="12" y2="23"></line><line x1="4.22" y1="4.22" x2="5.64" y2="5.64"></line><line x1="18.36" y1="18.36" x2="19.78" y2="19.78"></line><line x1="1" y1="12" x2="3" y2="12"></line><line x1="21" y1="12" x2="23" y2="12"></line><line x1="4.22" y1="19.78" x2="5.64" y2="18.36"></line><line x1="18.36" y1="5.64" x2="19.78" y2="4.22"></line></svg>
                        )}
                    </button>

                    {isLoggedIn ? (
                        <div className={styles.userMenuContainer}>
                            <div className={styles.avatar} onClick={() => setIsDropdownOpen(!isDropdownOpen)}>
                                JD
                            </div>
                            {isDropdownOpen && (
                                <div className={styles.dropdown}>
                                    <Link to="/profile" className={styles.dropdownItem} style={{display:'block',textDecoration:'none'}}>My Profile</Link>
                                    <button className={styles.dropdownItem}>My Lists</button>
                                    <button className={styles.dropdownItem}>Settings</button>
                                    <div className={styles.dropdownDivider}></div>
                                    <button className={styles.dropdownItem} onClick={() => setIsLoggedIn(false)}>Log Out</button>
                                </div>
                            )}
                        </div>
                    ) : (
                        <Link to="/signup" className={styles.btnLogin}>Log In</Link>
                    )}
                </nav>

            {/* Quick search modal skeleton */}
            {isSearchOpen && (
                <div className={styles.searchModalBackdrop} onClick={() => setIsSearchOpen(false)}>
                    <div className={styles.searchModal} onClick={e => e.stopPropagation()}>
                        <div className={styles.searchInputWrap}>
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                            <input type="text" placeholder="Search books, authors, lists…" autoFocus className={styles.searchInputActual} />
                            <span className={styles.escHint}>ESC</span>
                        </div>
                        <div className={styles.searchResults}>
                            <p className={styles.searchTitle}>Trending Searches</p>
                            <div className={styles.searchItem}>The Creative Act</div>
                            <div className={styles.searchItem}>Sci-Fi Classics</div>
                            <div className={styles.searchItem}>Best of 2023</div>
                        </div>
                    </div>
                </div>
            )}
        </header>
    );
};
