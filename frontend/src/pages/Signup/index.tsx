import { Link } from 'react-router-dom';
import styles from './Signup.module.css';
import { MagneticButton } from '../../components/MagneticButton';

export default function Signup() {
  return (
    <div className={styles.page}>
      <div className={styles.modal} role="dialog" aria-labelledby="signup-title">
        <aside className={styles.visual}>
          <div className={`${styles.glow} ${styles.glowA}`}></div>
          <div className={`${styles.glow} ${styles.glowB}`}></div>
          <div className={`${styles.glow} ${styles.glowC}`}></div>
          
          <div className={styles.bookStack}>
            <div className={`${styles.stackBook} ${styles.sb1}`}>
                <span className={styles.stackBookTitle}>The Salt House</span>
            </div>
            <div className={`${styles.stackBook} ${styles.sb2}`}>
                <span className={styles.stackBookTitle}>Orbital</span>
            </div>
            <div className={`${styles.stackBook} ${styles.sb3}`}>
                <span className={styles.stackBookTitle}>Piranesi</span>
            </div>
          </div>
          
          <div className={styles.visualCopy}>
            <h2>Your next chapter starts here</h2>
            <p>Join readers who share reviews, build lists, and find the books that stay with them.</p>
          </div>
        </aside>

        <div className={styles.formArea}>
          <Link to="/" className={styles.close} aria-label="Close">
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
              <path d="M4 4l8 8M12 4l-8 8" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round"/>
            </svg>
          </Link>

          <p className={styles.kicker}>Pagefold</p>
          <h1 className={styles.title} id="signup-title">Create your account</h1>
          <p className={styles.sub}>Already reading with us? <Link to="/login">Log in</Link></p>

          <form onSubmit={(e) => e.preventDefault()}>
            <div className={styles.field}>
              <label htmlFor="name">Display name</label>
              <input id="name" name="name" type="text" autoComplete="nickname" placeholder="How readers will see you" required />
            </div>
            <div className={styles.field}>
              <label htmlFor="email">Email</label>
              <input id="email" name="email" type="email" autoComplete="email" placeholder="you@example.com" required />
            </div>
            <div className={styles.field}>
              <label htmlFor="password">Password</label>
              <input id="password" name="password" type="password" autoComplete="new-password" placeholder="At least 8 characters" minLength={8} required />
            </div>
            <MagneticButton type="submit" variant="accent" style={{ width: '100%', marginTop: '8px' }}>Sign Up</MagneticButton>
          </form>
          <p className={styles.legal}>By signing up you agree to our <a href="#">Terms</a> and <a href="#">Privacy Policy</a>.</p>
        </div>
      </div>
    </div>
  );
}
